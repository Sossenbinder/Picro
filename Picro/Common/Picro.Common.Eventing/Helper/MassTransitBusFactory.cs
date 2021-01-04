using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Logging;
using Picro.Common.Utils.Retry;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Picro.Common.Eventing.Helper
{
    public static class MassTransitBusFactory
    {
        /// <summary>
        /// Creates a MT Bus with retry logic included (in case the RabbitMQ endpoint starts up later)
        /// </summary>
        public static IBusControl CreateBus(
            [NotNull] ILogger logger,
            Action<IRabbitMqBusFactoryConfigurator>? configFunc = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(async config =>
            {
                config.Durable = false;
                config.AutoDelete = true;
                config.PurgeOnStartup = true;

                await RetryStrategy.DoRetryExponential(() =>
                {
                    config.Host("rabbitmq://rabbitmq-picro");
                }, retryCount =>
                {
                    logger.LogInformation($"Retrying RabbitMQ setup for the {retryCount}# time");
                });

                configFunc?.Invoke(config);
            });
        }
    }
}