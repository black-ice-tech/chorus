namespace Chorus.CQRS
{
    public interface ICommand : ICorrelatable, IVersionable
    {
    }
}
