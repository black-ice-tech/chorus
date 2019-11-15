using Chorus.CQRS;
using Chorus.Samples.RestApi.Handlers;
using System;
using System.Threading.Tasks;

namespace Chorus.Samples.RestApi.Projectors
{
    public class PolicyInMemoryStoreProjector :
        IEventProjector<PolicyStarted>,
        IEventProjector<PolicyEnded>,
        IEventProjector<PolicyIssued>,
        IEventProjector<PolicyCancelled>
    {
        public Task ApplyAsync(PolicyCancelled evt)
        {
            InMemoryPolicyStore.Policies[evt.PolicyId].State = PolicyState.Cancelled;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(PolicyStarted evt)
        {
            InMemoryPolicyStore.Policies[evt.PolicyId].State = PolicyState.Started;
            InMemoryPolicyStore.Policies[evt.PolicyId].StartDate = DateTime.Today;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(PolicyEnded evt)
        {
            InMemoryPolicyStore.Policies[evt.PolicyId].State = PolicyState.Expired;
            InMemoryPolicyStore.Policies[evt.PolicyId].EndDate = DateTime.Today;
            return Task.CompletedTask;
        }

        public Task ApplyAsync(PolicyIssued evt)
        {
            InMemoryPolicyStore.Policies[evt.PolicyId] = new Policy { PolicyId = evt.PolicyId, CustomerId = Guid.NewGuid(), State = PolicyState.Issued };
            return Task.CompletedTask;
        }
    }
}
