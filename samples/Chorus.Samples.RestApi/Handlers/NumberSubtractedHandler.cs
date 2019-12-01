using System.Threading.Tasks;
using Chorus.CQRS;
using Microsoft.Extensions.Logging;

namespace Chorus.Samples.RestApi.Handlers
{
    public class NumberSubtractedHandler : IEventHandler<NumberSubtracted>
    {
        private readonly ILogger<NumberSubtractedHandler> _logger;

        public NumberSubtractedHandler(ILogger<NumberSubtractedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(NumberSubtracted evt)
        {
            _logger.LogInformation($"Handling event: {evt.Id}");

            return Task.CompletedTask;
        }
    }
}
