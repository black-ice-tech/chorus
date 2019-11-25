using System;
using Chorus.DistributedLog.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Chorus.DistributedLog.TextFile.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextFileDistributedLog(this IServiceCollection services, Action<TextFileDistributedLogOptions> action)
        {
            var options = new TextFileDistributedLogOptions();
            action.Invoke(options);

            services.AddSingleton<IDistributedLog, TextFileDistributedLog>();
            services.AddSingleton(options);
            return services;
        }
    }
}
