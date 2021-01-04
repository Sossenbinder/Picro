using Autofac;
using Picro.Common.SignalR.Caches;
using Picro.Common.SignalR.Caches.Interface;

namespace Picro.Common.SignalR.DI
{
    public class SignalRModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectedGroupCache>()
                .As<IConnectedGroupCache>()
                .SingleInstance();
        }
    }
}