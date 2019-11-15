using Chorus.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi
{
    public class NumberInMemoryStoreProjector : 
        IEventProjector<NumberAdded>,
        IEventProjector<NumberMultiplied>,
        IEventProjector<NumberSubtracted>
    {
        public Task ApplyAsync(NumberAdded evt)
        {
            InMemoryNumberStore.CurrentNum += evt.Num;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(NumberMultiplied evt)
        {
            InMemoryNumberStore.CurrentNum *= evt.Num;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(NumberSubtracted evt)
        {
            InMemoryNumberStore.CurrentNum -= evt.Num;
            return Task.CompletedTask;
        }
    }
}
