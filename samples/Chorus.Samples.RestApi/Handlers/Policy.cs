using Chorus.CQRS;
using System;

namespace Chorus.Samples.RestApi.Handlers
{
    public abstract class BasePolicyEvent : IEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CorrelationId { get; set; } = Guid.NewGuid();

        public Guid CausationId { get; set; } = Guid.NewGuid();

        public int Version { get; set; } = 1;

        public Guid PolicyId { get; set; }
    }

    public class PolicyIssued : BasePolicyEvent
    {
        public Guid CustomerId { get; set; }
    }

    public class PolicyStarted : BasePolicyEvent { }

    public class PolicyCancelled : BasePolicyEvent { }

    public class PolicyEnded : BasePolicyEvent { }

    public class Policy
    {
        public Guid PolicyId { get; set; }

        public Guid CustomerId { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public PolicyState State { get; set; }
    }

    public enum PolicyState
    {
        Issued,
        Started,
        Expired,
        Cancelled
    }
}
