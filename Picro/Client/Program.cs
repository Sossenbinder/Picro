using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Picro.Client.Communication;
using Picro.Client.Communication.Interface;
using Picro.Client.Services;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.Material;

namespace Picro.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			PopulateMsDiServices(builder.Services);

			builder.Configuration.AddInMemoryCollection(GenerateConfigs());
			builder.ConfigureContainer(new AutofacServiceProviderFactory(cb => PopulateContainer(builder, cb)));

			builder.RootComponents.Add<App>("#app");

			var builtHost = builder.Build();

			builtHost
				.Services
				.UseBootstrapProviders()
				.UseMaterialIcons();

			await builtHost.Services.GetRequiredService<ISessionService>().InitializeSession();

			await builtHost.RunAsync();
		}

		private static void PopulateMsDiServices(IServiceCollection services)
		{
			services.AddHttpClient(HttpClients.PicroBackend, (ctx, client) =>
			{
				client.BaseAddress = new Uri(ctx.GetRequiredService<IConfiguration>()["RemoteEndpoint"]);
			});

			services
				.AddBlazorise()
				.AddBootstrapProviders()
				.AddMaterialIcons();
		}

		private static void PopulateContainer(WebAssemblyHostBuilder wasmHostBuilder, ContainerBuilder builder)
		{
			builder.RegisterType<GlobalClickHandler>()
				.AsSelf()
				.SingleInstance();

			builder.RegisterInstance(new HttpClient { BaseAddress = new Uri(wasmHostBuilder.HostEnvironment.BaseAddress) })
				.As<HttpClient>();

			builder.RegisterType<KeepAliveService>()
				.As<IKeepAliveService>()
				.SingleInstance();

			builder.RegisterType<SessionService>()
				.As<ISessionService>()
				.SingleInstance();

			builder.RegisterType<PersonalImageService>()
				.As<IPersonalImageService>()
				.SingleInstance();

			builder.RegisterType<RequestMessageFactory>()
				.As<IRequestMessageFactory>()
				.SingleInstance();

			builder.RegisterType<SharedImageService>()
				.As<ISharedImageService>()
				.SingleInstance();

			builder.RegisterType<NotificationsService>()
				.As<INotificationsService>()
				.SingleInstance();
		}

		private static IDictionary<string, string> GenerateConfigs()
		{
			var dict = new Dictionary<string, string>();

			dict.Add("RemoteEndpoint", "https://localhost:1442");

			return dict;
		}
	}
}