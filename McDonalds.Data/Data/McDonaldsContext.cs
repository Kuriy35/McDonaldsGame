using McDonalds.Models;
using Microsoft.EntityFrameworkCore;

namespace McDonalds.Data
{
    public class McDonaldsContext : DbContext
    {
        public McDonaldsContext(DbContextOptions<McDonaldsContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Resource> Resources => Set<Resource>();
        public DbSet<DifficultyResource> DifficultyResources => Set<DifficultyResource>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DifficultyResource>()
                .HasKey(dr => new { dr.Difficulty, dr.ResourceName });

            modelBuilder.Entity<Resource>()
                .HasKey(r => r.Name);
        }
    }
}