using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Picro.Module.Image.DataTypes.Entity;

namespace Picro.Module.Image.Storage.Context
{
    public class ImageContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<ImageUserMappingEntity> ImageUserMapping { get; set; } = null!;

        public ImageContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageUserMappingEntity>().HasKey(x => new
            {
                x.UserId,
                x.ImageId
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class ImageDbContextFactory : IDbContextFactory<ImageContext>
    {
        private readonly string _connectionString;

        public ImageDbContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration["SqlConnectionString"];
        }

        public ImageContext CreateDbContext() => new(_connectionString);
    }
}