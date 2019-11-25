using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Moq;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog
{
    [TestFixture]
    public class StreamConsumerSpecs
    {
        private Mock<IDistributedLog> _log = new Mock<IDistributedLog>();
        private IStreamConsumer _consumer;

        [SetUp]
        public void SetUp()
        {
            _consumer = new StreamConsumer(_log.Object);
        }

        [Test]
        public async Task Should_be_able_to_consume_from_a_stream()
        {
            _log
                .Setup(log => log.RetrieveEntryAsync("test-stream", 0))
                .ReturnsAsync(() => Encoding.UTF8.GetBytes("some stuff"));

            await foreach (var msg in _consumer.ConsumeAsync("test-stream"))
            {
                Assert.That(Encoding.UTF8.GetString(msg), Is.EqualTo("some stuff"));
                Assert.Pass();
            }
        }

        [Test]
        public async Task Should_be_able_to_consume_from_an_empty_stream()
        {
            _log
                .Setup(log => log.RetrieveEntryAsync("test-stream", 0))
                .ReturnsAsync(() => null);

            var options = new ConsumerOptions.Builder()
                .StopConsumingAtEOF()
                .Build();

            await foreach (var _ in _consumer.ConsumeAsync("test-stream", options))
            {
                Assert.Fail("Stream is empty, should not consume anything");
            }

            Assert.Pass();
        }
    }
}
