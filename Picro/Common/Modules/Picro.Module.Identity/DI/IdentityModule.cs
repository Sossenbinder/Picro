using Autofac;
using Picro.Module.Identity.Cache;
using Picro.Module.Identity.Cache.Interface;
using Picro.Module.Identity.Service;
using Picro.Module.Identity.Service.Interface;
using Picro.Module.Identity.Storage;
using Picro.Module.Identity.Storage.Interface;

namespace Picro.Module.Identity.DI
{
    public class IdentityModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserIdentityService>()
                .As<IUserIdentityService>()
                .SingleInstance();

            builder.RegisterType<TableStorageIdentityStorageService>()
                .As<IIdentityStorageService>()
                .SingleInstance();

            builder.RegisterType<ConnectedUserCache>()
                .As<IConnectedUserCache>()
                .SingleInstance();
        }
    }
}