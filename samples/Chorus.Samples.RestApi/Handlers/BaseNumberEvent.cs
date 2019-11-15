using Chorus.CQRS;
using System;

namespace Chorus.Samples.RestApi
{
    public abstract class BaseNumberEvent : IEvent
    {
        public Guid Id { get; set; }

        public Guid CorrelationId { get; set; }

        public Guid CausationId { get; set; }

        public int Version { get; set; }

        public int Num { get; set; }
    }

    public class NumberAdded : BaseNumberEvent { }
    public class NumberSubtracted : BaseNumberEvent { }
    public class NumberMultiplied : BaseNumberEvent { }
}
