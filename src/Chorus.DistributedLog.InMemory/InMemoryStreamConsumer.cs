using Chorus.Core;
using Chorus.DistributedLog.Abstractions;
using System.Collections.Generic;

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
            return ConsumeAsync(streamName, null);
        }

        public async IAsyncEnumerable<byte[]> ConsumeAsync(string streamName, ConsumerOptions options)
        {
            streamName?.ThrowIfNull(nameof(streamName));
            var consumerOptions = new ConsumerOptions.Builder().Build();

            if (options != null)
            {
                consumerOptions = options;
            }

            _currentOffset = consumerOptions.StartOffset;

            var keepConsuming = true;

            while (keepConsuming)
            {
                var result = await _log.RetrieveEntryAsync(streamName, _currentOffset);

                if (result != null)
                {
                    yield return result;
                    _currentOffset++;
                }
                else if (consumerOptions.StopConsumingAtEOF)
                {
                    keepConsuming = false;
                }
            }
        }
    }
}
