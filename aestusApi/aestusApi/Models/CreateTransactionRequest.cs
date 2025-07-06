namespace aestusApi.Models;
using AnomalyDetection.Domain.Entities;

public class CreateTransactionRequest
{
    public required string UserId { get; init; }
    public required decimal Amount { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string Location { get; init; }

    public Transaction ToEntity() => new()
    {
        UserId = UserId,
        Amount = Amount,
        UtcTimestamp = Timestamp,
        Location = Location
    };
}
