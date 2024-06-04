using Microsoft.EntityFrameworkCore;
using Services.DataContext.Entities;

namespace Services.DataContext
{
    public class SpatialDbContext : DbContext
    {
        public SpatialDbContext() { }

        public SpatialDbContext(DbContextOptions options) : base(options) { }

        public DbSet<SpatialEntity> SpatialEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SpatialEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<SpatialEntity>()
            .Property(e => e.Polygon)
            .HasColumnType("geography");
        }
    }
}
