using Chorus.CQRS;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi.Handlers
{
    public class NumberAddedHandler : IEventHandler<NumberAdded>
    {
        private readonly ILogger<NumberAddedHandler> _logger;

        public NumberAddedHandler(ILogger<NumberAddedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(NumberAdded evt)
        {
            _logger.LogInformation($"Handling event: {evt.Id}");

            return Task.CompletedTask;
        }
    }
}
