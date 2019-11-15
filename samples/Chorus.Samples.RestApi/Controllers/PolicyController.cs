using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.Extensions;
using Chorus.Samples.RestApi.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IDistributedLog _distributedLog;
        private readonly ITopicNamingConvention _namingConvention;
        private readonly ILogger<PolicyController> _logger;

        public PolicyController(IDistributedLog distributedLog, ITopicNamingConvention namingConvention, ILogger<PolicyController> logger)
        {
            _distributedLog = distributedLog;
            _namingConvention = namingConvention;
            _logger = logger;
        }

        [HttpGet("{policyId}")]
        public Task<Policy> GetPolicy(Guid policyId)
        {
            return Task.FromResult(InMemoryPolicyStore.Policies[policyId]);
        }

        [HttpGet("start/{policyId}")]
        public async Task StartPolicy(Guid policyId)
        {
            _logger.LogInformation("Starting policy {PolicyId}", policyId);

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<PolicyStarted>(), new PolicyStarted { PolicyId = policyId });
        }

        [HttpGet("issue")]
        public async Task IssuePolicy()
        {
            var policyId = Guid.NewGuid();

            _logger.LogInformation("Issuing new policy: {PolicyId}", policyId);

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<PolicyIssued>(), new PolicyIssued { PolicyId =  policyId });
        }

        [HttpGet("cancel")]
        public async Task CancelPolicy(Guid policyId)
        {
            _logger.LogInformation("Cancelling policy: {PolicyId}", policyId);

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<PolicyCancelled>(), new PolicyCancelled { PolicyId = policyId });
        }

        [HttpGet("end")]
        public async Task EndPolicy(Guid policyId)
        {
            _logger.LogInformation("Ending policy: {PolicyId}", policyId);

            await _distributedLog.AppendAsync(_namingConvention.GetTopicName<PolicyEnded>(), new PolicyEnded { PolicyId = policyId });
        }
    }
}