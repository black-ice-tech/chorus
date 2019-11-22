using Chorus.DistributedLog.Abstractions;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace Chorus.DistributedLog.Extensions
{
    public static class StronglyTypedDistributedLogExtensions
    {
        public static Task AppendAsync<T>(this IDistributedLog distributedLog, string streamName, T payload)
        {
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

            return distributedLog.AppendAsync(streamName, bytes);
        }

        public static async Task<T> RetrieveEntry<T>(this IDistributedLog distributedLog, string streamName, int offset)
        {
            var bytes = await distributedLog.RetrieveEntryAsync(streamName, offset);

            var payload = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject<T>(payload);
        }
    }
}
