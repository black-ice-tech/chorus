using System;
using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.InMemory;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog
{
    [TestFixture]
    public abstract class BaseDistributedLogSpecs
    {
        protected static readonly string StreamName = "test-stream";
        protected IDistributedLog _log;

        public abstract void SetUp();

        [Test]
        public async Task Should_be_able_to_append_to_a_stream_and_retrieve_entry()
        {
            await _log.AppendAsync(StreamName, Encoding.UTF8.GetBytes("some stuff"));

            byte[] entry = await _log.RetrieveEntryAsync(StreamName, 0);

            Assert.That(Encoding.UTF8.GetString(entry), Is.EqualTo("some stuff"));
        }

        [Test]
        public async Task Should_get_null_if_stream_is_empty()
        {
            byte[] entry = await _log.RetrieveEntryAsync(StreamName, 0);

            Assert.That(entry, Is.Null);
        }

        [Test]
        public async Task Should_be_able_to_retrieve_entry_from_any_valid_offset()
        {
            await _log.AppendAsync(StreamName, Encoding.UTF8.GetBytes($"some stuff 0"));
            await _log.AppendAsync(StreamName, Encoding.UTF8.GetBytes($"some stuff 1"));
            await _log.AppendAsync(StreamName, Encoding.UTF8.GetBytes($"some stuff 2"));
            await _log.AppendAsync(StreamName, Encoding.UTF8.GetBytes($"some stuff 3"));

            byte[] entryOffset2 = await _log.RetrieveEntryAsync(StreamName, 2);
            byte[] entryOffset0 = await _log.RetrieveEntryAsync(StreamName, 0);
            byte[] entryOffset1 = await _log.RetrieveEntryAsync(StreamName, 1);
            byte[] entryOffset3 = await _log.RetrieveEntryAsync(StreamName, 3);

            Assert.That(Encoding.UTF8.GetString(entryOffset0), Is.EqualTo("some stuff 0"));
            Assert.That(Encoding.UTF8.GetString(entryOffset1), Is.EqualTo("some stuff 1"));
            Assert.That(Encoding.UTF8.GetString(entryOffset2), Is.EqualTo("some stuff 2"));
            Assert.That(Encoding.UTF8.GetString(entryOffset3), Is.EqualTo("some stuff 3"));
        }

        [Test]
        public async Task Should_get_argument_exception_if_offset_is_negative()
        {
            Assert.ThrowsAsync<ArgumentException>(() => _log.RetrieveEntryAsync(StreamName, -1));
        }
    }
}
