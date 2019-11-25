using System;
using Chorus.CQRS;

namespace Chorus.Specs.Messaging
{
    public class MyEvent : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid CorrelationId { get; } = Guid.NewGuid();
        public Guid CausationId { get; } = Guid.NewGuid();
        public int Version { get; } = 1;
    }
}
