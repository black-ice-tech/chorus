using System;
using Chorus.DistributedLog.Abstractions;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.InMemory
{
    public class InMemoryDistributedLog : IDistributedLog
    {
        private static readonly string OffsetMustBePositive = "Offset must be positive";
        private readonly ConcurrentDictionary<string, List<byte[]>> _streams = new ConcurrentDictionary<string, List<byte[]>>();

        public Task AppendAsync(string streamName, byte[] payload)
        {
            if (!_streams.ContainsKey(streamName))
            {
                _streams[streamName] = new List<byte[]>();
            }

            _streams[streamName].Add(payload);

            return Task.CompletedTask;
        }

        public Task<byte[]> RetrieveEntryAsync(string streamName, int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentException(OffsetMustBePositive, nameof(offset));
            }

            if (!_streams.ContainsKey(streamName))
            {
                _streams[streamName] = new List<byte[]>();
            }

            if (offset >= _streams[streamName].Count)
            {
                return Task.FromResult<byte[]>(null);
            }

            return Task.FromResult(_streams[streamName][offset]);
        }
    }
}
