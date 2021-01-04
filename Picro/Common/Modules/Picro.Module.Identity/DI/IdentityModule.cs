using Autofac;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Identity.Cache;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.Service;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Identity.Storage;
using Picro.Module.Identity.Storage.Context;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.DI
{
    public class IdentityModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .SingleInstance();

            builder.RegisterType<UserContextFactory>()
                .As<IDbContextFactory<UserContext>>()
                .SingleInstance();

            builder.RegisterType<SqlUserStorageRepository>()
                .As<IUserRepository>()
                .SingleInstance();

            builder.RegisterType<UserCache>()
                .As<IUserCache>()
                .SingleInstance();
        }
    }
}