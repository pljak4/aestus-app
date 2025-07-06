using System.Diagnostics;
using AnomalyDetection.Domain.Entities;
using AnomalyDetection.Infrastructure;
using AnomalyDetection.Application.Services;
using aestusApi.Controllers;
using aestusApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AnomalyDetection.Tests;

public class PerformanceTests
{
    private static AppDbContext InMemoryDb()
    {
        var opt = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(opt);
    }

    [Fact]
    public async Task Should_Handle_1000_Transactions_Under_1_Second()
    {

        var db = InMemoryDb();
        var detector = new AnomalyDetectorService();  
        var logger = LoggerFactory.Create(b => b.AddDebug())
                                  .CreateLogger<TransactionController>();

        var controller = new TransactionController(db, detector, logger);

        var tasks = new List<Task>();

        var sw = Stopwatch.StartNew();

        for (int i = 0; i < 1000; i++)
        {
            var req = new CreateTransactionRequest
            {
                UserId = "perf_user",
                Amount = i % 2 == 0 ? 500 : 1500,
                Timestamp = DateTime.UtcNow,
                Location = i % 3 == 0 ? "Zagreb" : "Split"
            };

            tasks.Add(controller.CreateTransaction(req));
        }

        await Task.WhenAll(tasks);

        sw.Stop();


        Assert.True(sw.ElapsedMilliseconds <= 1000, $"Obrada je trajala {sw.ElapsedMilliseconds} ms, a mora biti ≤ 1000 ms.");
    }
}
