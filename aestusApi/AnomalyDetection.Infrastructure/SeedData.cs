using AnomalyDetection.Domain.Entities;

namespace AnomalyDetection.Infrastructure;

public static class SeedData
{
    public static void Load(AppDbContext db)
    {
        if (db.Transactions.Any()) return;

        var list = new List<Transaction>
        {
            new()
            {
                Id = Guid.Parse("07690894-1575-4e65-b107-04843b16ea06"),
                UserId = "1",
                Amount = 2312310.00m,
                UtcTimestamp = DateTime.Parse("2025-07-03T13:09:52.567Z"),
                Location = "Sarajevo",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("8b9fe96a-09d1-458c-a39e-2b73ab02a113"),
                UserId = "1",
                Amount = 32322.00m,
                UtcTimestamp = DateTime.Parse("2025-06-26T10:37:45.779Z"),
                Location = "Rijeka",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("58805023-5670-41fa-b0de-7ef6a20a77b3"),
                UserId = "1",
                Amount = 32322.00m,
                UtcTimestamp = DateTime.Parse("2025-06-26T10:37:45.779Z"),
                Location = "Zadar",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("2e579f5f-5c02-422b-8346-930bf0bbf872"),
                UserId = "1",
                Amount = 869.00m,
                UtcTimestamp = DateTime.Parse("2025-06-26T10:37:45.779Z"),
                Location = "Split",
                IsSuspicious = true,
                Comment = "Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("5901ac96-10a4-4f0b-82bd-c17ba7f2fd45"),
                UserId = "1",
                Amount = 225000.00m,
                UtcTimestamp = DateTime.Parse("2024-04-24T09:07:00Z"),
                Location = "Sarajevo",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("9c389374-5fb5-4f02-8d68-ce1f6c600121"),
                UserId = "1",
                Amount = 3233330.00m,
                UtcTimestamp = DateTime.Parse("2025-07-05T17:00:04.723Z"),
                Location = "Sarajevo",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("0df78e68-5b3b-4ecf-b0a0-d2ea67d4cb01"),
                UserId = "1",
                Amount = 1869.00m,
                UtcTimestamp = DateTime.Parse("2025-06-16T10:37:45.779Z"),
                Location = "Beograd",
                IsSuspicious = true,
                Comment = "Visok iznos + Neočekivana lokacija"
            },
            new()
            {
                Id = Guid.Parse("086a5e78-4da2-4240-b341-d9827a635ef3"),
                UserId = "1",
                Amount = 89,
                UtcTimestamp = DateTime.Parse("2025-07-03T13:07:29.937Z"),
                Location = "Sarajevo",
                IsSuspicious = false,
            }
        };

        db.Transactions.AddRange(list);
        db.SaveChanges();
    }
}
