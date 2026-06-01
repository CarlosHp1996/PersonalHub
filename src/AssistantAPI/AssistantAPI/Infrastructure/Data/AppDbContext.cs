using AssistantAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssistantAPI.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Bill> Bills => Set<Bill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(e => {
            e.HasKey(b => b.Id);
            e.Property(b => b.Name).HasMaxLength(255).IsRequired();
            e.Property(b => b.Amount).HasPrecision(10, 2);
            e.Property(b => b.Status).HasConversion<string>();
        });
    }
}
