using System;
using Chorus.CQRS;

namespace Chorus.Specs.Messaging
{
    public class MyEvent : IEvent
    {
        public Guid Id { get; }
        public Guid CorrelationId { get; }
        public Guid CausationId { get; }
        public int Version { get; }
        public string MyString { get; set; }
    }
}
