using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.Abstractions
{
    public interface IStreamConsumer
    {
        IAsyncEnumerable<byte[]> ConsumeAsync(string streamName);

        IAsyncEnumerable<byte[]> ConsumeAsync(string streamName, ConsumerOptions options);
    }
}
