using Chorus.Samples.RestApi.Handlers;
using System;
using System.Collections.Generic;

namespace Chorus.Samples.RestApi
{
    public static class InMemoryPolicyStore
    {
        public static Dictionary<Guid, Policy> Policies = new Dictionary<Guid, Policy>();
    }
}
