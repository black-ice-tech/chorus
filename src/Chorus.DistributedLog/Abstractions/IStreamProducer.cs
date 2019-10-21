using System.Threading.Tasks;

namespace Chorus.DistributedLog.Abstractions
{
    public interface IStreamProducer
    {
        Task SendAsync<T>(string streamName, T payload);
    }
}
