using System;

namespace Chorus.DistributedLog.Abstractions
{
    public interface ITopicNamingConvention
    {
        /// <summary>
        /// Return the topic name based on the convention for type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get the topic name for.</typeparam>
        /// <returns>A string of the topic name.</returns>
        string GetTopicName<T>();

        /// <summary>
        /// Return the topic name based on the convention for type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to get the topic name for.</param>
        /// <returns>A string of the topic name.</returns>
        string GetTopicName(Type type);
    }
}
