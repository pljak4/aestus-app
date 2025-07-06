using AnomalyDetection.Domain.Entities;
using AnomalyDetection.Infrastructure;
using aestusApi.Controllers;
using aestusApi.Models;
using AnomalyDetection.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AnomalyDetection.Tests;

public class TransactionControllerTests
{
    private static AppDbContext InMemDb()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(opts);
    }

    [Fact]
    public async Task CreateTransaction_ReturnsCreated_WhenValid()
    {
        var db = InMemDb();
        var detector = new AnomalyDetectorService();
        var logger = NullLogger<TransactionController>.Instance;
        var controller = new TransactionController(db, detector, logger);

        var req = new CreateTransactionRequest
        {
            UserId = "user123",
            Amount = 1500,
            Timestamp = DateTime.UtcNow,
            Location = "Split"
        };

        var result = await controller.CreateTransaction(req);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var txn = Assert.IsType<Transaction>(created.Value);
        Assert.True(txn.IsSuspicious);
        Assert.Equal("user123", txn.UserId);
    }

    [Fact]
    public async Task GetAnomaliesForUser_ReturnsOnlySuspicious()
    {
        var db = InMemDb();
        var detector = new AnomalyDetectorService();
        var logger = NullLogger<TransactionController>.Instance;
        var ctrl = new TransactionController(db, detector, logger);

        db.Transactions.AddRange(
            new Transaction { UserId = "u1", Amount = 50, UtcTimestamp = DateTime.UtcNow, Location = "Zagreb", IsSuspicious = false },
            new Transaction { UserId = "u1", Amount = 2000, UtcTimestamp = DateTime.UtcNow, Location = "Osijek", IsSuspicious = true }
        );
        await db.SaveChangesAsync();

        var result = await ctrl.GetAnomaliesForUser("u1");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var list = Assert.IsAssignableFrom<List<Transaction>>(ok.Value);
        Assert.Single(list);
        Assert.True(list[0].IsSuspicious);
    }
}
