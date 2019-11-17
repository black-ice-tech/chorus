using Chorus.CQRS;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.InMemory.Extensions;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Chorus.Specs.DistributedLog.InMemory
{
    [TestFixture]
    public class ServiceCollectionExtensionsSpecs
    {
        [Test]
        [TestCase(typeof(IDistributedLog))]
        [TestCase(typeof(IStreamConsumer))]
        public void Should_be_able_to_add_all_required_in_memory_services(Type serviceType)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddInMemoryDistributedLog();

            var serviceProvider = services.BuildServiceProvider();

            Assert.DoesNotThrow(() => serviceProvider.GetRequiredService(serviceType));
        }
    }

    public class MyTestEvent : IEvent
    {
        public Guid Id { get; }

        public Guid CorrelationId { get; }

        public Guid CausationId { get; }

        public int Version { get; }
    }

    public class MyTestEventHandler : IEventHandler<MyTestEvent>
    {
        public Predicate<MyTestEvent> OnlyHandleIf => (evt) => true;

        public Task HandleAsync(MyTestEvent evt)
        {
            return Task.CompletedTask;
        }
    }
}
