using Chorus.DistributedLog;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog
{
    [TestFixture]
    public class ConsumerOptionsSpecs
    {
        [Test]
        public void Should_be_able_to_build_instance_from_builder()
        {
            ConsumerOptions options = new ConsumerOptions.Builder()
                .StopConsumingAtEOF()
                .FromOffset(12)
                .WithGroupId("my-group-id")
                .Build();

            Assert.That(options.StartOffset, Is.EqualTo(12));
            Assert.That(options.ConsumerGroupId, Is.EqualTo("my-group-id"));
            Assert.That(options.StopConsumingAtEOF, Is.True);
        }
    }
}
