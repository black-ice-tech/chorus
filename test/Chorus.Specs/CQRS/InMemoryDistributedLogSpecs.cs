using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.InMemory;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;

namespace Chorus.Specs.CQRS
{
    [TestFixture]
    public class InMemoryDistributedLogSpecs
    {
        private IDistributedLog _log;
        private IStreamConsumer _consumer;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _log = new InMemoryDistributedLog();
            _consumer = new StreamConsumer(_log);
        }

        [Test]
        public async Task Should_be_able_to_append_logs_to_a_stream_and_consume()
        {
            var payloads = new[] { "abc", "def", "ghi" };

            await _log.AppendAsync("my-stream", Encoding.UTF8.GetBytes(payloads[0]));
            await _log.AppendAsync("my-stream", Encoding.UTF8.GetBytes(payloads[1]));
            await _log.AppendAsync("my-stream", Encoding.UTF8.GetBytes(payloads[2]));

            var options = new ConsumerOptions.Builder()
                .StopConsumingAtEOF()
                .Build();

            var expectedPayloadIndex = 0;

            await foreach (var logEntry in _consumer.ConsumeAsync("my-stream", options))
            {
                var payload = logEntry != null ? Encoding.UTF8.GetString(logEntry) : null;
                Assert.That(payload, Is.EqualTo(payloads[expectedPayloadIndex]));
                expectedPayloadIndex++;
            }
        }
    }
}
