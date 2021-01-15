using Autofac;
using Picro.Common.Eventing.Events.MassTransit;
using Picro.Common.Eventing.Events.MassTransit.Interface;

namespace Picro.Common.Eventing.DI
{
	public class EventingModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<MassTransitSignalRBackplaneService>()
				.As<IMassTransitSignalRBackplaneService>()
				.SingleInstance();

			builder.RegisterType<MassTransitEventingService>()
				.As<IMassTransitEventingService>()
				.SingleInstance();
		}
	}
}