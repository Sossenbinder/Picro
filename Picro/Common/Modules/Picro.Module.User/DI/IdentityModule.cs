using Autofac;
using Microsoft.EntityFrameworkCore;
using Picro.Module.User.Cache;
using Picro.Module.User.Cache.Interface;
using Picro.Module.User.Service;
using Picro.Module.User.Service.Interface;
using Picro.Module.User.Storage;
using Picro.Module.User.Storage.Context;
using Picro.Module.User.Storage.Interface;

namespace Picro.Module.User.DI
{
    public class IdentityModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .SingleInstance();

            builder.RegisterType<UserContextFactory>()
                .As<IDbContextFactory<UserDbContext>>()
                .SingleInstance();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .SingleInstance();

            builder.RegisterType<UserCache>()
                .As<IUserCache>()
                .SingleInstance();

            builder.RegisterType<NotificationSubscriptionRepository>()
                .As<INotificationSubscriptionRepository>()
                .SingleInstance();
        }
    }
}