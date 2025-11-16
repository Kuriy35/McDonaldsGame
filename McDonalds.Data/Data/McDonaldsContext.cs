using McDonalds.Models;
using Microsoft.EntityFrameworkCore;

namespace McDonalds.Data
{
    public class McDonaldsContext : DbContext
    {
        public McDonaldsContext(DbContextOptions<McDonaldsContext> options)
            : base(options)
        {
            //Database.Migrate();
        }

        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<DifficultyResource> DifficultyResources => Set<DifficultyResource>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DifficultyResource>(entity =>
            {
                entity.HasKey(e => new { e.Difficulty, e.ResourceName });

                entity.Property(e => e.BuyPrice)
                      .HasPrecision(18, 2);  

                entity.Property(e => e.SellPrice)
                      .HasPrecision(18, 2);  
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.Property(e => e.BuyPrice)
                      .HasPrecision(18, 2);  

                entity.Property(e => e.SellPrice)
                      .HasPrecision(18, 2);  
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}