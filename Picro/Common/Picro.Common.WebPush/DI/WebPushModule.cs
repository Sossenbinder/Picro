using Autofac;
using Picro.Common.WebPush.Service;
using Picro.Common.WebPush.Service.Interface;

namespace Picro.Common.WebPush.DI
{
    public class WebPushModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebPushService>()
                .As<IWebPushService>()
                .SingleInstance();
        }
    }
}