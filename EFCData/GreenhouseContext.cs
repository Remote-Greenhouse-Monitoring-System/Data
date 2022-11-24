﻿using Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCData;

public class GreenhouseContext : DbContext
{
    public DbSet<Measurement>? Measurements { get; set; } = null!;
    public DbSet<PlantProfile>? PlantProfiles { get; set; } = null!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=tcp:greenhouse-db-server.database.windows.net,1433;Initial Catalog=GreenhouseDB;Persist Security Info=False;User ID=viasep4;Password=Vanaheim_Perplex1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Measurement>().HasKey(m => m.Id);
        modelBuilder.Entity<PlantProfile>().HasKey(p => p.Id);
    }
}