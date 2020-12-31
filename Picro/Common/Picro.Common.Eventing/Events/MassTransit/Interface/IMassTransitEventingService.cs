using System;
using System.Threading.Tasks;
using MassTransit;
using Picro.Common.Eventing.DataTypes;

namespace Picro.Common.Eventing.Events.MassTransit.Interface
{
    public interface IMassTransitEventingService
    {
        Task RaiseEvent<T>(T message)
            where T : class;

        void RegisterForEvent<T>(string queueName, Action<T> handler, QueueType queueType = QueueType.RegularQueue)
            where T : class;

        void RegisterForEvent<T>(string queueName, Func<T, Task> handler, QueueType queueType = QueueType.RegularQueue)
            where T : class;

        void RegisterConsumer<TConsumer>(string queueName, TConsumer consumer, QueueType queueType = QueueType.RegularQueue)
            where TConsumer : class, IConsumer;
    }
}