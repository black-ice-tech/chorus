using System.Threading.Tasks;

namespace Chorus.CQRS
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        Task HandleAsync(TEvent evt);
    }
}
