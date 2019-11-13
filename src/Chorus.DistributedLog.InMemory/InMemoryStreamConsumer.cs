using Chorus.Core;
using Chorus.DistributedLog.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.InMemory
{
    public class InMemoryStreamConsumer : IStreamConsumer
    {
        private readonly IDistributedLog _log;
        private int _currentOffset = 0;

        public InMemoryStreamConsumer(IDistributedLog log)
        {
            _log = log;
        }

        public IAsyncEnumerable<byte[]> ConsumeAsync(string streamName)
        {
            return ConsumeAsync(streamName, new ConsumerOptions.Builder().Build());
        }

        public async IAsyncEnumerable<byte[]> ConsumeAsync(string streamName, ConsumerOptions options)
        {
            streamName?.ThrowIfNull(nameof(streamName));
            options?.ThrowIfNull(nameof(options));

            var keepConsuming = true;

            while (keepConsuming)
            {
                var result = await _log.RetrieveEntry(streamName, _currentOffset);

                if (result != null)
                {
                    yield return result;
                    _currentOffset++;
                }
                else if (options.StopConsumingAtEOF)
                {
                    keepConsuming = false;
                }
            }
        }
    }
}
