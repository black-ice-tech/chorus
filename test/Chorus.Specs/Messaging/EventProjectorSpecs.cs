using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.Messaging;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ILogger = Castle.Core.Logging.ILogger;

namespace Chorus.Specs.Messaging
{
    [TestFixture]
    public class EventProjectorSpecs
    {
        private readonly Mock<IStreamConsumer> _streamConsumer = new Mock<IStreamConsumer>();
        private readonly Mock<ILogger<EventProjector<MyEvent>>> _log = new Mock<ILogger<EventProjector<MyEvent>>>();
        private EventProjector<MyEvent> _eventProjector;

        [SetUp]
        public void SetUp()
        {
            _streamConsumer.Setup(consumer => consumer.ConsumeAsync("my-event", It.IsAny<ConsumerOptions>()))
                .Returns(GetFakeAsyncStream);

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton<IEventApplier<MyEvent>, MyEventApplier>();
            services.AddSingleton<ITopicNamingConvention, KebabCaseTopicNamingConvention>();
            services.AddSingleton(_streamConsumer.Object);

            var sp = services.BuildServiceProvider();

            _eventProjector = new EventProjector<MyEvent>(
                sp.GetRequiredService<IStreamConsumer>(),
                sp.GetRequiredService<ITopicNamingConvention>(),
                sp.GetRequiredService<IEnumerable<IEventApplier<MyEvent>>>(),
                _log.Object);
        }

        [TearDown]
        public void TearDown()
        {
            MyEventApplier.AppliedEvent = null;
        }

        [Test]
        public async Task Should_be_able_to_consume_event_and_route_to_handler()
        {
            var tokenSource = new CancellationTokenSource();

            await _eventProjector.StartAsync(tokenSource.Token);
            await Task.Delay(100, tokenSource.Token);
            tokenSource.Cancel();

            Assert.That(MyEventApplier.AppliedEvent, Is.Not.Null);
        }

        [Test]
        public async Task Should_exit_gracefully_on_exception()
        {
            _streamConsumer.Setup(consumer => consumer.ConsumeAsync("my-event", It.IsAny<ConsumerOptions>()))
                .Returns((IAsyncEnumerable<byte[]>) null);

            var tokenSource = new CancellationTokenSource();

            await _eventProjector.StartAsync(tokenSource.Token);

            Assert.That(_log.Invocations.Count, Is.GreaterThanOrEqualTo(1));

            Assert.That(MyEventApplier.AppliedEvent, Is.Null);
        }

        private async IAsyncEnumerable<byte[]> GetFakeAsyncStream()
        {
            var serializedEvent = JsonConvert.SerializeObject(new MyEvent {MyString = "abc"});
            yield return Encoding.UTF8.GetBytes(serializedEvent);
        }
    }

    public class MyEventApplier : IEventApplier<MyEvent>
    {
        public static MyEvent AppliedEvent = null;
        public Predicate<MyEvent> OnlyHandleIf { get; } = null;

        public Task ApplyAsync(MyEvent evt)
        {
            AppliedEvent = evt;
            return Task.CompletedTask;
        }
    }
}
