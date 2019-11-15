using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chorus.CQRS
{
    public interface IEventProjector<TEvent>
        where TEvent : IEvent
    {
        Task ApplyAsync(TEvent evt);
    }
}
