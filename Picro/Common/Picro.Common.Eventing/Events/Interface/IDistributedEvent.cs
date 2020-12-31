using System;
using System.Threading.Tasks;
using MassTransit;
using Picro.Common.Disposable;

namespace Picro.Common.Eventing.Events.Interface
{
    public interface IDistributedEvent<TEventArgs>
        where TEventArgs : class
    {
        Task Raise(TEventArgs eventArgs);

        void RaiseFireAndForget(TEventArgs eventArgs);

        DisposableAction RegisterForErrors(Func<Fault<TEventArgs>, Task> faultHandler);

        DisposableAction Register(Func<TEventArgs, Task> handler);

        void UnRegister(Func<TEventArgs, Task> handler);
    }
}