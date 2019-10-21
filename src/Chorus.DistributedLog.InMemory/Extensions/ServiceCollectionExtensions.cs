using Chorus.DistributedLog.Abstractions;
using Chorus.Messaging;
using Chorus.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chorus.DistributedLog.InMemory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInMemoryDistributedLog(this IServiceCollection services, params Type[] markerTypes)
        {
            services.AddSingleton<IDistributedLog, InMemoryDistributedLog>();
            services.AddTransient<IStreamConsumer, InMemoryStreamConsumer>();
            services.AddSingleton<ITopicNamingConvention, KebabCaseTopicNamingConvention>();
            services.AddEventHandlersAndConsumers(markerTypes);
        }

        private static void AddEventHandlersAndConsumers(this IServiceCollection services, params Type[] markerTypes)
        {
            var eventHandlers = new List<Type>();
            foreach (var type in markerTypes)
            {
                eventHandlers.AddRange(GetAllTypesImplementingOpenGenericType(typeof(IEventHandler<>), type.Assembly));
            }

            foreach (var handlerType in eventHandlers)
            {
                var interfaces = handlerType.GetInterfaces().Where(t => t.GetGenericTypeDefinition() == typeof(IEventHandler<>)).ToList();

                foreach (var interfaceType in interfaces)
                {
                    services.AddTransient(interfaceType, handlerType);

                    var evtType = interfaceType.GetGenericArguments()[0];
                    var consumerType = typeof(EventConsumer<>).MakeGenericType(evtType);

                    // TODO - there's got to be a better way to do this
                    var method = typeof(ServiceCollectionHostedServiceExtensions).GetMethods()
                        .Where(m => m.IsStatic && m.IsPublic)
                        .First(m => m.Name == "AddHostedService")
                        .MakeGenericMethod(consumerType);

                    method.Invoke(null, new object[] { services });
                }
            }
        }

        // Taken from stack overflow
        private static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, Assembly assembly)
        {
            return from x in assembly.GetTypes()
                   from z in x.GetInterfaces()
                   let y = x.BaseType
                   where
                   (y != null && y.IsGenericType &&
                   openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
                   (z.IsGenericType &&
                   openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
                   select x;
        }
    }
}
