using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Models.DB_First;

public partial class McDonaldsContext : DbContext
{
    public McDonaldsContext()
    {
    }

    public McDonaldsContext(DbContextOptions<McDonaldsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DifficultyResource> DifficultyResources { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DifficultyResource>(entity =>
        {
            entity.HasKey(e => new { e.Difficulty, e.ResourceName });

            entity.Property(e => e.BuyPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.BuyPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellPrice).HasColumnType("decimal(18, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
