using System.Threading.Tasks;
using Chorus.CQRS;
using Chorus.Samples.RestApi.Handlers;

namespace Chorus.Samples.RestApi.Appliers
{
    public class CountAllNumbersOver100 :
        IEventApplier<NumberAdded>,
        IEventApplier<NumberSubtracted>
    {
        public Task ApplyAsync(NumberAdded evt)
        {
            if (evt.Num > 100)
            {
                InMemoryNumberStore.NumbersOver100Count++;
            }

            return Task.CompletedTask;
        }

        public Task ApplyAsync(NumberSubtracted evt)
        {
            if (evt.Num > 100)
            {
                InMemoryNumberStore.NumbersOver100Count++;
            }

            return Task.CompletedTask;
        }
    }
}
