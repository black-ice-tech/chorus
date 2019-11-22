using System.Threading.Tasks;

namespace Chorus.DistributedLog.Abstractions
{
    public interface IDistributedLog
    {
        Task AppendAsync(string streamName, byte[] payload);

        Task<byte[]> RetrieveEntryAsync(string stream, int offset);
    }
}
