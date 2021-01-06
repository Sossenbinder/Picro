using Autofac;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Connection.Storage;
using Picro.Module.Notification.Storage.Context;
using Picro.Module.Notification.Storage.Interface;

namespace Picro.Module.Connection.DI
{
    public class ConnectionModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationDbContextFactory>()
                .As<IDbContextFactory<NotificationDbContext>>()
                .SingleInstance();

            builder.RegisterType<NotificationSubscriptionRepository>()
                .As<INotificationSubscriptionRepository>()
                .SingleInstance();
        }
    }
}