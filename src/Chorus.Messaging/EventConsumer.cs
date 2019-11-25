using Chorus.CQRS;
using Chorus.DistributedLog.Abstractions;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chorus.Messaging
{
    public class EventConsumer<TEvent> : BackgroundService
        where TEvent : IEvent
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITopicNamingConvention _topicNamingConvention;
        private readonly ILogger<EventConsumer<TEvent>> _logger;

        public EventConsumer(IServiceProvider serviceProvider, ITopicNamingConvention topicNamingConvention, ILogger<EventConsumer<TEvent>> logger)
        {
            _serviceProvider = serviceProvider;
            _topicNamingConvention = topicNamingConvention;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting event consumer for type {EventType}", typeof(TEvent).Name);
            var topic = _topicNamingConvention.GetTopicName<TEvent>();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumer = (IStreamConsumer)_serviceProvider.GetService(typeof(IStreamConsumer));

                // For some reason, without adding another "await" statement, the below async stream blocks
                // the main thread and won't start up the application. This is a workaround
                await Task.Delay(10, stoppingToken);

                await foreach (var msg in consumer.ConsumeAsync(topic).WithCancellation(stoppingToken))
                {
                    var obj = JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(msg));
                    var handler = (IEventHandler<TEvent>)_serviceProvider.GetService(typeof(IEventHandler<TEvent>));
                    await handler.HandleAsync(obj);
                }
            }
        }
    }
}
