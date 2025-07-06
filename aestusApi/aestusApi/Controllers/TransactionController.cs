using AnomalyDetection.Domain.Entities;
using AnomalyDetection.Infrastructure;
using aestusApi.Models;
using AnomalyDetection.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aestusApi.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AnomalyDetectorService _detector;
    private readonly ILogger<TransactionController> _logger;

    public TransactionController(AppDbContext db, AnomalyDetectorService detector, ILogger<TransactionController> logger)
    {
        _db = db;
        _detector = detector;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        _logger.LogInformation("POST /transactions {User} {Amt} {Loc}", request.UserId, request.Amount, request.Location);

        var transaction = request.ToEntity();
        var (isSuspicious, comment) = _detector.Analyze(transaction);

        transaction.IsSuspicious = isSuspicious;
        transaction.Comment = comment;

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Transaction>> GetTransaction(Guid id)
    {
        var txn = await _db.Transactions.FindAsync(id);
        return txn is null ? NotFound() : Ok(txn);
    }

    [HttpGet("/users/{userId}/anomalies")]
    public async Task<ActionResult<List<Transaction>>> GetAnomaliesForUser(string userId)
    {
        var suspicious = await _db.Transactions
            .Where(t => t.UserId == userId && t.IsSuspicious)
            .ToListAsync();

        return Ok(suspicious);
    }

    [HttpGet("/users/{userId}/anomalies/stats")]
    public async Task<ActionResult<IEnumerable<object>>> GetAnomalyStats(string userId)
    {
        var stats = await _db.Transactions
            .Where(t => t.UserId == userId && t.IsSuspicious)
            .GroupBy(t => t.UtcTimestamp.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        return Ok(stats);
    }
}
