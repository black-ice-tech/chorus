using Chorus.Core;
using Chorus.DistributedLog.Abstractions;
using System;

namespace Chorus.DistributedLog
{
    public class KebabCaseTopicNamingConvention : ITopicNamingConvention
    {
        public string GetTopicName<T>()
        {
            return GetTopicName(typeof(T));
        }

        public string GetTopicName(Type type)
        {
            var typeName = type?.Name;

            return typeName.PascalToKebabCase();
        }
    }
}
