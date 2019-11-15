using System.Text;
using System.Threading.Tasks;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.Extensions;
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
        private readonly ITopicNamingConvention _namingConvention;
        private readonly ILogger<NumberController> _logger;

        public NumberController(IDistributedLog distributedLog, ITopicNamingConvention namingConvention, ILogger<NumberController> logger)
        {
            _distributedLog = distributedLog;
            _namingConvention = namingConvention;
            _logger = logger;
        }

        [HttpGet]
        public async Task<int> GetCurrent()
        {
            return InMemoryNumberStore.CurrentNum;
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

        [HttpGet("multiply/{num}")]
        public async Task Multiply(int num)
        {
            _logger.LogInformation("Multiplying number...");

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<NumberMultiplied>(), new NumberMultiplied { Num = num });
        }
    }
}
