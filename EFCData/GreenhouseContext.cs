using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class GreenhouseContext : DbContext
{
    public DbSet<Measurement>? Measurements { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-A0MGCBJ;Database=GreenhouseTest;User Id=Mihoi;Password=12345;TrustServerCertificate=True;Trusted_Connection=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>().HasKey(m => m.Id);
    }
}