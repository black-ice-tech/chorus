using System.Collections.Generic;
using System.Linq;
using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.Extensions;
using Chorus.DistributedLog.InMemory.Extensions;
using Chorus.Messaging.Abstractions;
using Chorus.Specs.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog
{
    [TestFixture]
    public class ServiceCollectionExtensionsSpecs
    {
        [Test]
        public void Should_be_able_to_add_chorus_dependencies()
        {
            var services = new ServiceCollection()
                .AddChorus(typeof(MyEvent))
                .AddInMemoryDistributedLog()
                .AddLogging();

            var sp = services.BuildServiceProvider();

            var eventHandler = sp.GetRequiredService<IEventHandler<MyEvent>>();
            var eventAppliers = sp.GetRequiredService<IEnumerable<IEventApplier<MyEvent>>>();
            var namingConvention = sp.GetRequiredService<ITopicNamingConvention>();
            var hostedServices = sp.GetRequiredService<IEnumerable<IHostedService>>();

            Assert.That(eventHandler, Is.InstanceOf<MyEventHandler>());
            Assert.That(eventAppliers.First(), Is.InstanceOf<MyEventApplier>());
            Assert.That(namingConvention, Is.InstanceOf<KebabCaseTopicNamingConvention>());
            Assert.That(hostedServices, Is.Not.Empty);
        }
    }
}
