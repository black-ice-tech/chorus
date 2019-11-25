using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog
{
    [TestFixture]
    public class KebabCaseTopicNamingConventionSpecs
    {
        private ITopicNamingConvention _topicNamingConvention = new KebabCaseTopicNamingConvention();

        [Test]
        public void Should_be_able_to_convert_class_name_to_kebab_case()
        {
            string name = _topicNamingConvention.GetTopicName<KebabCaseTopicNamingConventionSpecs>();

            Assert.That(name, Is.EqualTo("kebab-case-topic-naming-convention-specs"));
        }
    }
}
