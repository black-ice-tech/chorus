using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.Extensions;
using Chorus.Samples.RestApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chorus.Samples.RestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NumberController : ControllerBase
    {
        private readonly IDistributedLog _distributedLog;
        private readonly IStreamConsumer _consumer;
        private readonly ITopicNamingConvention _namingConvention;
        private readonly ILogger<NumberController> _logger;

        public NumberController(
            IDistributedLog distributedLog, 
            IStreamConsumer consumer,
            ITopicNamingConvention namingConvention, ILogger<NumberController> logger)
        {
            _distributedLog = distributedLog;
            _consumer = consumer;
            _namingConvention = namingConvention;
            _logger = logger;
        }

        [HttpGet]
        public int GetCurrent()
        {
            return InMemoryNumberStore.CurrentNum;
        }
        
        [HttpGet("full")]
        public async Task<int> ReadFull()
        {
            var num = 0;

            var options = new ConsumerOptions.Builder()
                .StopConsumingAtEOF()
                .Build();

            await foreach (var msg in _consumer.ConsumeAsync("number-added", options))
            {
                var msgStr = Encoding.UTF8.GetString(msg);
                num += JsonConvert.DeserializeObject<NumberAdded>(msgStr).Num;
            }
            
            await foreach (var msg in _consumer.ConsumeAsync("number-subtracted", options))
            {
                num -= JsonConvert.DeserializeObject<NumberAdded>(Encoding.UTF8.GetString(msg)).Num;
            }

            return num;
        }

        [HttpGet("count100")]
        public int GetCount()
        {
            return InMemoryNumberStore.NumbersOver100Count;
        }

        [HttpGet("add/{num}")]
        public async Task Add(int num)
        {
            _logger.LogInformation("Adding number...");

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<NumberAdded>(), new NumberAdded { Num = num });
        }

        [HttpGet("subtract/{num}")]
        public async Task Subtract(int num)
        {
            _logger.LogInformation("Subtracting number...");

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<NumberSubtracted>(), new NumberSubtracted { Num = num });
        }
    }
}
