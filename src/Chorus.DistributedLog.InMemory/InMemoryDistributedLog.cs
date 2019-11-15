using Chorus.DistributedLog.Abstractions;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.InMemory
{
    public class InMemoryDistributedLog : IDistributedLog
    {
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

        public Task<byte[]> RetrieveEntry(string stream, int offset)
        {
            if (!_streams.ContainsKey(stream))
            {
                _streams[stream] = new List<byte[]>();
            }

            if (!(offset >= 0 && offset < _streams[stream].Count)) return Task.FromResult<byte[]>(null);

            return Task.FromResult(_streams[stream][offset]);
        }
    }
}
