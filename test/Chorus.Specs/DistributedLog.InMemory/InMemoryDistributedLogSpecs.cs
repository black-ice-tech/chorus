using Chorus.DistributedLog.InMemory;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog.InMemory
{
    [TestFixture]
    public class InMemoryDistributedLogSpecs : BaseDistributedLogSpecs
    {
        [SetUp]
        public override void SetUp()
        {
            _log = new InMemoryDistributedLog();
        }
    }
}
