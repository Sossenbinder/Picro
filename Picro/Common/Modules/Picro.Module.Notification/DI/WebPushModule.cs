using Autofac;
using Microsoft.Extensions.Configuration;
using Picro.Module.Notification.Service;
using Picro.Module.Notification.Service.Interface;
using Picro.Module.User.Storage;
using Picro.Module.User.Storage.Interface;
using WebPush;

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

            builder.RegisterType<NotificationSubscriptionRepository>()
                .As<INotificationSubscriptionRepository>()
                .SingleInstance();

            builder.Register(ctx =>
                {
                    var configuration = ctx.Resolve<IConfiguration>();
                    return new VapidDetails(configuration["VapidSubject"], configuration["VapidPublicKey"], configuration["VapidPrivateKey"]);
                })
                .As<VapidDetails>()
                .SingleInstance();

            builder.RegisterType<WebPushClient>()
                .As<WebPushClient>()
                .SingleInstance();
        }
    }
}