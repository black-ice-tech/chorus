using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;

namespace Chorus.DistributedLog.TextFile
{
    public class TextFileDistributedLog : IDistributedLog
    {
        public async Task AppendAsync(string streamName, byte[] payload)
        {
            await using var writer = new StreamWriter($"{streamName}.txt", append:true);

            await writer.WriteLineAsync(Encoding.UTF8.GetString(payload));
        }

        public async Task<byte[]> RetrieveEntryAsync(string stream, int offset)
        {
            using var reader = new StreamReader($"{stream}.txt");

            var line = await reader.ReadLineAsync();

            return Encoding.UTF8.GetBytes(line);
        }
    }
}
