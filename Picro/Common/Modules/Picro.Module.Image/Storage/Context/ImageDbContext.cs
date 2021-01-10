using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Picro.Module.Image.DataTypes.Entity;

namespace Picro.Module.Image.Storage.Context
{
    public class ImageDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<UploadedImageInfoEntity> ImageUserMappings { get; set; } = null!;

        public DbSet<ImageDistributionMappingEntity> ImageDistributionMappings { get; set; } = null!;

        public ImageDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageDistributionMappingEntity>().HasKey(x => new
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

    public class ImageDbContextFactory : IDbContextFactory<ImageDbContext>
    {
        private readonly string _connectionString;

        public ImageDbContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration["SqlConnectionString"];
        }

        public ImageDbContext CreateDbContext() => new(_connectionString);
    }
}