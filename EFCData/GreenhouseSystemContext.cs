using System.Data.Common;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace EFCData;

public class GreenhouseSystemContext : DbContext
{
    public DbSet<Measurement>? Measurements { get; set; } = null!;
    public DbSet<GreenHouse>? GreenHouses { get; set; } = null!;
    public DbSet<PlantProfile>? PlantProfiles { get; set; } = null!;
    public DbSet<User>? Users { get; set; } = null!;
    public DbSet<Threshold>? Thresholds { get; set; } = null!;

    protected readonly IConfiguration Configuration;
    

    public GreenhouseSystemContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("GreenhouseDB"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>().HasKey(m => m.Id);
        
        modelBuilder.Entity<GreenHouse>().HasKey(g => g.Id);
        modelBuilder.Entity<PlantProfile>().HasKey(p => p.Id);
        modelBuilder.Entity<GreenHouse>().HasIndex(g => g.DeviceEui).IsUnique();

        modelBuilder.Entity<User>().HasKey(u => u.Id);
        
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Threshold>().HasKey(t => t.Id);
    }
}