using System.Collections.Concurrent;
using AnomalyDetection.Application.Utilities;
using AnomalyDetection.Domain.Entities;

namespace AnomalyDetection.Application.Services
{
    public sealed class AnomalyDetectorService
    {
        private readonly ConcurrentDictionary<string, RunningStats> _statsByUser
            = new();

        private readonly ConcurrentDictionary<string, SlidingWindow> _windowByUser
            = new();

        private readonly ConcurrentDictionary<string, List<DateTime>> _timestampsByUser
            = new();

        public (bool IsSuspicious, string? Comment) Analyze(Transaction txn)
        {
            double amount = (double)txn.Amount;

            var stats = _statsByUser.GetOrAdd(txn.UserId, _ => new RunningStats());
            double z = stats.StdDev > 0
                ? (amount - stats.Mean) / stats.StdDev
                : 0.0;
            stats.Push(amount);

            if (Math.Abs(z) > 3.0)
                return (true, $"Ekstremni iznos (Z={z:F2})");

            var window = _windowByUser.GetOrAdd(txn.UserId, _ => new SlidingWindow(100));
            window.Push(amount);
            var (q1, q3) = window.GetQuartiles();
            if (!double.IsNaN(q1) && !double.IsNaN(q3))
            {
                double iqr = q3 - q1;
                double lower = q1 - 1.5 * iqr;
                double upper = q3 + 1.5 * iqr;
                if (amount < lower || amount > upper)
                    return (true, $"Iznos izvan IQR raspona [{lower:F2}, {upper:F2}]");
            }

            if (!string.Equals(txn.Location, "Zagreb", StringComparison.OrdinalIgnoreCase))
                return (true, "Neočekivana lokacija");

            var timestamps = _timestampsByUser.GetOrAdd(txn.UserId, _ => new List<DateTime>());
            timestamps.Add(txn.UtcTimestamp);
            if (timestamps.Count > 10)
                timestamps.RemoveAt(0);

            if (timestamps.Count >= 2)
            {
                var diffs = timestamps
                    .Zip(timestamps.Skip(1), (a, b) => (b - a).TotalSeconds)
                    .ToList();

                double avgSec = diffs.Average();
                if (avgSec < 1.0)
                    return (true, "Prevelika frekvencija tranzakcija");
            }

            return (false, null);
        }
    }
}
