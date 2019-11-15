using Chorus.DistributedLog.Abstractions;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.Extensions
{
    public static class GenericDistributedLogExtensions
    {
        public static Task AppendAsync<T>(this IDistributedLog distributedLog, string streamName, T payload)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

            return distributedLog.AppendAsync(streamName, bytes);
        }
    }
}
