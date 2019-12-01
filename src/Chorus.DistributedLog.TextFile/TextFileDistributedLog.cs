using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;

namespace Chorus.DistributedLog.TextFile
{
    public class TextFileDistributedLog : IDistributedLog
    {
        private static readonly int MaxEntrySize = 1024 * 1000;
        private readonly TextFileDistributedLogOptions _options;

        public TextFileDistributedLog(TextFileDistributedLogOptions options)
        {
            _options = options;
        }

        public async Task AppendAsync(string streamName, byte[] payload)
        {
            var filePath = Path.Combine(_options.StreamDirectory, $"{streamName}.txt");

            List<byte> payloadList = new List<byte>(MaxEntrySize);
            byte[] sizeInBytes = BitConverter.GetBytes(payload.Length);

            payloadList.AddRange(sizeInBytes);
            payloadList.AddRange(payload);

            var lineToWrite = payloadList.ToArray();

            Array.Resize(ref lineToWrite, MaxEntrySize);

            await using var outStream =
                new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

            outStream.Seek(0, SeekOrigin.End);

            var blah = Encoding.UTF8.GetString(lineToWrite);
            var length = lineToWrite.Length;
            await outStream.WriteAsync(lineToWrite, 0, lineToWrite.Length);
        }

        public async Task<byte[]> RetrieveEntryAsync(string streamName, int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset must be positive", nameof(offset));
            }

            var filePath = Path.Combine(_options.StreamDirectory, $"{streamName}.txt");

            await using var inStream =
                new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

            var byteOffset = offset * MaxEntrySize;
            inStream.Seek(byteOffset, SeekOrigin.Begin);
            byte[] entryLengthBuffer = new byte[4];
            await inStream.ReadAsync(entryLengthBuffer);
            var entryLength = BitConverter.ToInt32(entryLengthBuffer);
            byte[] buffer = new byte[entryLength];
            await inStream.ReadAsync(buffer);

            return buffer.Length > 0 ? buffer : null;
        }
    }
}
