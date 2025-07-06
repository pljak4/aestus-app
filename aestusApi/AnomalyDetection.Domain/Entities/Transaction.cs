namespace AnomalyDetection.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserId { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime UtcTimestamp { get; set; }
    public string Location { get; set; } = null!;
    public bool IsSuspicious { get; set; }
    public string? Comment { get; set; }

    public void MarkSuspicious(string reason)
    {
        IsSuspicious = true;
        Comment = reason;
    }
}
