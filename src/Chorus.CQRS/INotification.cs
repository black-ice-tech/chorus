namespace Chorus.CQRS
{
    public interface INotification : ICorrelatable, IVersionable
    {
    }
}
