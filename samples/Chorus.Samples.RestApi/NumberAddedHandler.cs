using Chorus.CQRS;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi
{
    public class NumberAdded : IEvent
    {
        public Guid Id { get; set; }

        public Guid CorrelationId { get; set; }

        public Guid CausationId { get; set; }

        public int Version { get; set; }

        public string MyString { get; set; }

        public int Num { get; set; }
    }

    public class NumberAddedHandler : IEventHandler<NumberAdded>
    {
        private readonly ILogger<NumberAddedHandler> _logger;

        public Predicate<NumberAdded> OnlyHandleIf => evt => evt.MyString == "papa murphy's";

        public NumberAddedHandler(ILogger<NumberAddedHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(NumberAdded evt)
        {
            _logger.LogInformation($"Handling event: {evt.Num}");
            return Task.CompletedTask;
        }
    }
}
