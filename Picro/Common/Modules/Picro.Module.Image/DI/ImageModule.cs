using Autofac;
using Microsoft.EntityFrameworkCore;
using Picro.Module.Image.Event;
using Picro.Module.Image.Event.Interface;
using Picro.Module.Image.Service;
using Picro.Module.Image.Service.Interface;
using Picro.Module.Image.Storage;
using Picro.Module.Image.Storage.Context;
using Picro.Module.Image.Storage.Interface;

namespace Picro.Module.Image.DI
{
    public class ImageModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageEventHub>()
                .As<IImageEventHub>()
                .SingleInstance();

            builder.RegisterType<ImageService>()
                .As<IImageService>()
                .SingleInstance();

            builder.RegisterType<ImageStorageService>()
                .As<IImageStorageService>()
                .SingleInstance();

            builder.RegisterType<ImageUserMappingSqlService>()
                .As<IImageUserMappingSqlService>()
                .SingleInstance();

            builder.RegisterType<ImageDbContextFactory>()
                .As<IDbContextFactory<ImageContext>>()
                .SingleInstance();

            builder.RegisterType<ImageDistributionService>()
                .As<IImageDistributionService>()
                .SingleInstance()
                .AutoActivate();
        }
    }
}