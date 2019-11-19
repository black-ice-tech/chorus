using System.Threading.Tasks;
using Chorus.DistributedLog.InMemory;
using Chorus.DistributedLog.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chorus.Specs.Extensions
{
    [TestFixture]
    public class StronglyTypedDistributedLogExtensionsSpecs
    {
        [Test]
        public async Task Should_be_able_to_append_strongly_typed_payload()
        {
            var inMemoryDistributedLog = new InMemoryDistributedLog();

            await inMemoryDistributedLog.AppendAsync("my-stream", new TestEvent {TestString = "123"});

            var testEvent = await inMemoryDistributedLog.RetrieveEntry<TestEvent>("my-stream", 0);

            Assert.That(testEvent.TestString, Is.EqualTo("123"));
        }

        [Test]
        public async Task Should_get_serialization_exception_on_mismatched_type()
        {
            var inMemoryDistributedLog = new InMemoryDistributedLog();

            await inMemoryDistributedLog.AppendAsync("my-stream", new {Abc = "123"});

            Assert.ThrowsAsync<JsonSerializationException>(() =>
                inMemoryDistributedLog.RetrieveEntry<NonDeserializableType>("my-stream", 0));
        }
    }

    public class TestEvent
    {
        public string TestString { get; set; }
    }

    public class NonDeserializableType
    {
        public string TestString { get; private set; }

        private NonDeserializableType(string abc)
        {
        }
    }
}
