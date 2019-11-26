using System.Threading.Tasks;

namespace Chorus.CQRS
{
    public interface IEventApplier
    {
    }

    public interface IEventApplier<in TEvent> : IEventApplier
        where TEvent : IEvent
    {
        Task ApplyAsync(TEvent evt);
    }
}
