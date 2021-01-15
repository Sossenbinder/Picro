using Autofac;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace Picro.Common.Storage.DI
{
	public class StorageModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(ctx =>
			{
				var connectionString = ctx.Resolve<IConfiguration>()["CloudStorageAccountConnectionString"];

				return CloudStorageAccount.Parse(connectionString);
			})
				.As<CloudStorageAccount>()
				.SingleInstance();

			builder.Register(ctx => ctx.Resolve<CloudStorageAccount>().CreateCloudTableClient())
				.As<CloudTableClient>()
				.SingleInstance();

			builder.Register(ctx =>
			{
				var connectionString = ctx.Resolve<IConfiguration>()["CloudStorageAccountConnectionString"];

				return new BlobServiceClient(connectionString);
			})
				.As<BlobServiceClient>()
				.SingleInstance();
		}
	}
}