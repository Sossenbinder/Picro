using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Picro.Module.Identity.DataTypes.Entity;

namespace Picro.Module.Identity.Storage.Context
{
    public class UserContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<UserEntity> Users { get; set; } = null!;

        public UserContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class UserContextFactory : IDbContextFactory<UserContext>
    {
        private readonly string _connectionString;

        public UserContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration["SqlConnectionString"];
        }

        public UserContext CreateDbContext() => new(_connectionString);
    }
}