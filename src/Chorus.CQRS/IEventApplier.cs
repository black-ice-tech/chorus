using System.Threading.Tasks;

namespace Chorus.CQRS
{
    public interface IEventApplier<TEvent>
        where TEvent : IEvent
    {
        Task ApplyAsync(TEvent evt);
    }
}
