using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chorus.Specs.Messaging
{
    [TestFixture]
    public class EventConsumerSpecs
    {
        private readonly Mock<IStreamConsumer> _streamConsumer = new Mock<IStreamConsumer>();
        private EventConsumer<MyEvent> _eventConsumer;

        [SetUp]
        public void SetUp()
        {
            _streamConsumer.Setup(consumer => consumer.ConsumeAsync("my-event"))
                .Returns(GetFakeAsyncStream);

            var services = new ServiceCollection();

            services.AddLogging();
            services.AddSingleton<IEventHandler<MyEvent>, MyEventHandler>();
            services.AddSingleton<ITopicNamingConvention, KebabCaseTopicNamingConvention>();
            services.AddSingleton(_streamConsumer.Object);

            var sp = services.BuildServiceProvider();

            _eventConsumer = new EventConsumer<MyEvent>(sp, sp.GetRequiredService<ITopicNamingConvention>(),
                sp.GetRequiredService<ILogger<EventConsumer<MyEvent>>>());
        }

        [TearDown]
        public void TearDown()
        {
            MyEventHandler.HandledEvent = null;
        }

        [Test]
        public async Task Should_be_able_to_consume_event_and_route_to_handler()
        {
            var tokenSource = new CancellationTokenSource();

            await _eventConsumer.StartAsync(tokenSource.Token);
            await Task.Delay(100, tokenSource.Token);
            tokenSource.Cancel();

            Assert.That(MyEventHandler.HandledEvent, Is.Not.Null);
        }

        private async IAsyncEnumerable<byte[]> GetFakeAsyncStream()
        {
            var serializedEvent = await Task.Run(() => JsonConvert.SerializeObject(new MyEvent()));
            yield return Encoding.UTF8.GetBytes(serializedEvent);
        }
    }

    public class MyEventHandler : IEventHandler<MyEvent>
    {
        public static MyEvent HandledEvent;

        public Task HandleAsync(MyEvent evt)
        {
            HandledEvent = evt;
            return Task.CompletedTask;
        }
    }
}
