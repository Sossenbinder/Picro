using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Picro.Module.Notification.DataTypes.Entity;

namespace Picro.Module.Notification.Storage.Context
{
    public class NotificationDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<NotificationSubscriptionEntity> NotificationSubscriptions { get; set; } = null!;

        public NotificationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class NotificationDbContextFactory : IDbContextFactory<NotificationDbContext>
    {
        private readonly string _connectionString;

        public NotificationDbContextFactory(IConfiguration configuration)
        {
            _connectionString = configuration["SqlConnectionString"];
        }

        public NotificationDbContext CreateDbContext() => new(_connectionString);
    }
}