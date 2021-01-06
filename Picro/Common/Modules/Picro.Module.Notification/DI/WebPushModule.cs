using Autofac;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Notification.Service;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.Notification.Storage;
using Picro.Module.Notification.Storage.Context;
using Picro.Module.Notification.Storage.Interface;

namespace Picro.Module.Notification.DI
{
    public class WebPushModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebPushService>()
                .As<IWebPushService>()
                .SingleInstance();

            builder.RegisterType<NotificationService>()
                .As<INotificationService>()
                .SingleInstance();

            builder.RegisterType<NotificationDbContextFactory>()
                .As<IDbContextFactory<NotificationDbContext>>()
                .SingleInstance();

            builder.RegisterType<NotificationSubscriptionRepository>()
                .As<INotificationSubscriptionRepository>()
                .SingleInstance();
        }
    }
}