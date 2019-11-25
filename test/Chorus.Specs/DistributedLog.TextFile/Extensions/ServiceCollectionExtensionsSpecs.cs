using System;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.InMemory.Extensions;
using Chorus.DistributedLog.TextFile;
using Chorus.DistributedLog.TextFile.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Chorus.Specs.DistributedLog.TextFile.Extensions
{
    [TestFixture]
    public class ServiceCollectionExtensionsSpecs
    {
        [Test]
        [TestCase(typeof(IDistributedLog), typeof(TextFileDistributedLog))]
        [TestCase(typeof(TextFileDistributedLogOptions), typeof(TextFileDistributedLogOptions))]
        public void Should_be_able_to_add_all_required_in_memory_services(Type serviceType, Type implementationType)
        {
            var services = new ServiceCollection();
            services.AddTextFileDistributedLog(options => options.StreamDirectory = string.Empty);

            var serviceProvider = services.BuildServiceProvider();
            var implementation = serviceProvider.GetRequiredService(serviceType);
            
            Assert.That(implementation, Is.InstanceOf(implementationType));
        }
    }
}
