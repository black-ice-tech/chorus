using Chorus.CQRS;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi
{
    public class MyEvent : IEvent
    {
        public Guid Id { get; set; }

        public Guid CorrelationId { get; set; }

        public Guid CausationId { get; set; }

        public int Version { get; set; }

        public string MyString { get; set; }
    }

    public class MyEventHandler : IEventHandler<MyEvent>
    {
        private readonly ILogger<MyEventHandler> _logger;

        public Predicate<MyEvent> OnlyHandleIf => evt => evt.MyString == "papa murphy's";

        public MyEventHandler(ILogger<MyEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(MyEvent evt)
        {
            _logger.LogInformation($"Handling event: {evt.GetType()}");
            return Task.CompletedTask;
        }
    }
}
