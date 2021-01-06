using Autofac;
using MassTransit;
using MassTransit.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Picro.Common.Eventing.DI;
using Picro.Common.Eventing.Helper;
using Picro.Common.SignalR.DI;
using Picro.Common.SignalR.Hubs;
using Picro.Common.Storage.DI;
using Picro.Module.User.DI;
using Picro.Module.Image.DI;
using System;
using System.Threading.Tasks;
using Picro.Module.Connection.DI;
using Picro.Module.Notification.DI;

namespace Picro.Server
{
	public class Startup
	{
		private const string LocalCors = "LocalCors";

		private readonly IWebHostEnvironment _webHostEnvironment;

		public Startup(
			IConfiguration configuration,
			IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddRazorPages();
			services.AddSignalR();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Events.OnRedirectToLogin = context =>
					{
						context.Response.StatusCode = 401;
						return Task.CompletedTask;
					};

					options.ExpireTimeSpan = TimeSpan.FromDays(100 * 365);
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.MaxAge = TimeSpan.FromDays(100 * 365);
					options.Cookie.SameSite = _webHostEnvironment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict;
					options.Cookie.Name = Configuration["IdentificationCookieName"];
				});

			services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()));
			services.AddAntiforgery(x => x.HeaderName = "AntiForgery");
			services.AddResponseCompression();

			ConfigureMassTransit(services);

			if (_webHostEnvironment.IsDevelopment())
			{
				services.AddCors(corsOption =>
				{
					corsOption.AddPolicy(LocalCors,
						builder =>
						{
							builder
								.WithOrigins("https://localhost:5001")
								.AllowAnyHeader()
								.WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Delete)
								.AllowCredentials();
						}
					);
				});
			}
		}

		private static void ConfigureMassTransit(IServiceCollection services)
		{
			services.AddSignalR();

			// creating the bus config
			services.AddMassTransit(x =>
			{
				x.AddSignalRHub<SessionHub>();

				x.AddBus(registrationContext => MassTransitBusFactory.CreateBus(registrationContext.GetService<ILogger<Startup>>(), cfg =>
				{
					cfg.ConfigureEndpoints(registrationContext);
				}));
			});
		}

		// ReSharper disable once UnusedMember.Global
		public void ConfigureContainer(ContainerBuilder builder)
		{
			builder.RegisterModule<IdentityModule>();
			builder.RegisterModule<StorageModule>();
			builder.RegisterModule<ImageModule>();
			builder.RegisterModule<EventingModule>();
			builder.RegisterModule<SignalRModule>();
			builder.RegisterModule<WebPushModule>();
			builder.RegisterModule<ConnectionModule>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				if (_webHostEnvironment.IsDevelopment())
				{
					app.UseWebAssemblyDebugging();
					app.UseCors(LocalCors);
				}
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();

				// Route to serve blazor app
				//endpoints.MapFallbackToFile("index.html");

				endpoints.MapHub<SessionHub>("/SessionHub");

				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}