using System;

namespace Chorus.CQRS
{
    public interface ICorrelatable
    {
        Guid Id { get; }
        Guid CorrelationId { get; }
        Guid CausationId { get; }
    }
}
