namespace Chorus.CQRS
{
    public interface IEvent : ICorrelatable, IVersionable
    {
    }
}
