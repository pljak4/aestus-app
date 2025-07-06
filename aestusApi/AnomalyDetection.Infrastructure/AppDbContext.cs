using AnomalyDetection.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Transactions;
using Transaction = AnomalyDetection.Domain.Entities.Transaction;

namespace AnomalyDetection.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.UserId).IsRequired();
            entity.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            entity.Property(t => t.UtcTimestamp).IsRequired();
            entity.Property(t => t.Location).IsRequired();
            entity.Property(t => t.IsSuspicious).HasDefaultValue(false);
            entity.Property(t => t.Comment).HasMaxLength(256);
        });
    }
}
