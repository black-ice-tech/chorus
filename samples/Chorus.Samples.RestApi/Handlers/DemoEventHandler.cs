using Chorus.CQRS;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi.Handlers
{
    public class DemoEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        private readonly ILogger<DemoEventHandler<TEvent>> _logger;

        public DemoEventHandler(ILogger<DemoEventHandler<TEvent>> logger)
        {
            _logger = logger;
        }

        public Predicate<TEvent> OnlyHandleIf => null;

        public Task HandleAsync(TEvent evt)
        {
            _logger.LogInformation($"Handling event: {evt.Id}");

            return Task.CompletedTask;
        }
    }
}
