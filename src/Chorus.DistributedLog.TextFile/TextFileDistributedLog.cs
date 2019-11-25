using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;

namespace Chorus.DistributedLog.TextFile
{
    public class TextFileDistributedLog : IDistributedLog
    {
        private readonly TextFileDistributedLogOptions _options;
        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public TextFileDistributedLog(TextFileDistributedLogOptions options)
        {
            _options = options;
        }

        public async Task AppendAsync(string streamName, byte[] payload)
        {
            var filePath = Path.Combine(_options.StreamDirectory, $"{streamName}.txt");

            _readWriteLock.EnterWriteLock();

            try
            {
                if (!File.Exists(filePath))
                {
                    await using var _ = File.Create(filePath);
                }

                await using StreamWriter sw = File.AppendText(filePath);
                sw.WriteLine(Encoding.UTF8.GetString(payload));
                sw.Close();
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public async Task<byte[]> RetrieveEntryAsync(string streamName, int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Offset must be positive", nameof(offset));
            }

            var filePath = Path.Combine(_options.StreamDirectory, $"{streamName}.txt");

            string line = null;

            _readWriteLock.EnterReadLock();

            try
            {
                if (!File.Exists(filePath))
                {
                    await using var _ = File.Create(filePath);
                }

                line = File.ReadLines(filePath).Skip(offset).Take(1).FirstOrDefault();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }

            return line is null ? null : Encoding.UTF8.GetBytes(line);
        }
    }
}
