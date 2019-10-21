using Chorus.CQRS;
using System;
using System.Threading.Tasks;

namespace Chorus.Messaging.Abstractions
{
    public interface IEventHandler
    {
        Task HandleAsync(IEvent evt);
    }

    public interface IEventHandler<TEvent>
        where TEvent : IEvent
    {
        Predicate<TEvent> OnlyHandleIf { get; }

        Task HandleAsync(TEvent evt);
    }

    public interface ICar
    {
        void StartEngine();
        void StopEngine();
        bool Has4WheelDrive { get; }
        bool IsConvertible { get; }
    }
}
