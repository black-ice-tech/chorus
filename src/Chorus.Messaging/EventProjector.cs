using System;
using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chorus.Messaging
{
    // TODO - is this the right naespace for this?
    public class EventProjector<TEvent> : BackgroundService
        where TEvent : IEvent
    {
        private readonly IStreamConsumer _streamConsumer;
        private readonly ITopicNamingConvention _topicNamingConvention;
        private readonly IEnumerable<IEventApplier<TEvent>> _eventAppliers;
        private readonly ILogger<EventProjector<TEvent>> _logger;
        private ConsumerOptions _options;

        public EventProjector(
            IStreamConsumer streamConsumer,
            ITopicNamingConvention topicNamingConvention,
            IEnumerable<IEventApplier<TEvent>> eventAppliers,
            ILogger<EventProjector<TEvent>> logger)
        {
            _streamConsumer = streamConsumer;
            _topicNamingConvention = topicNamingConvention;
            _eventAppliers = eventAppliers;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting event projector for type {EventType}", typeof(TEvent).Name);
            var topic = _topicNamingConvention.GetTopicName<TEvent>();

            while (!stoppingToken.IsCancellationRequested)
            {
                // For some reason, without adding another "await" statement, the below async stream blocks
                // the main thread and won't start up the application. This is a workaround
                await Task.Delay(10, stoppingToken);

                try
                {
                    await foreach (var msg in _streamConsumer.ConsumeAsync(topic, _options ?? new ConsumerOptions()).WithCancellation(stoppingToken))
                    {
                        var obj = JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(msg));

                        var tasks = new List<Task>();

                        foreach (IEventApplier<TEvent> applier in _eventAppliers)
                        {
                            tasks.Add(applier.ApplyAsync(obj));
                        }

                        await Task.WhenAll(tasks);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogCritical("Projector {ProjectorName} failed. Reason: {Exception}", GetType().FullName,
                        e.Message);
                    return;
                }
            }
        }
    }
}
