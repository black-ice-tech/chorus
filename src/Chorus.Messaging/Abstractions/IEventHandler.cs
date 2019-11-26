using Chorus.CQRS;
using System;
using System.Threading.Tasks;

namespace Chorus.Messaging.Abstractions
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<in TEvent> : IEventHandler
        where TEvent : IEvent
    {
        Task HandleAsync(TEvent evt);
    }
}
