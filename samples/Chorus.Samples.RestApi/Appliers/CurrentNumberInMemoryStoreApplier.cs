using Chorus.CQRS;
using Chorus.Samples.RestApi.Handlers;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi.Appliers
{
    public class CurrentNumberInMemoryStoreApplier :
        IEventApplier<NumberAdded>,
        IEventApplier<NumberSubtracted>
    {
        public Task ApplyAsync(NumberAdded evt)
        {
            InMemoryNumberStore.CurrentNum += evt.Num;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(NumberSubtracted evt)
        {
            InMemoryNumberStore.CurrentNum -= evt.Num;
            return Task.CompletedTask;
        }
    }
}
