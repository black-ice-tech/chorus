using Chorus.CQRS;
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
        public static IServiceCollection AddInMemoryDistributedLog(this IServiceCollection services)
        {
            services.AddSingleton<IDistributedLog, InMemoryDistributedLog>();
            return services;
        }

//        private static void AddProjectors(this IServiceCollection services, params Type[] markerTypes)
//        {
//            var projectors = new List<Type>();
//            foreach (var type in markerTypes)
//            {
//                projectors.AddRange(GetAllTypesImplementingOpenGenericType(typeof(IEventApplier<>), type.Assembly));
//            }
//
//            foreach (var projectorType in projectors)
//            {
//                var interfaces = projectorType.GetInterfaces()
//                    .Where(t => t.GetGenericTypeDefinition() == typeof(IEventApplier<>))
//                    .ToList();
//
//                foreach (var interfaceType in interfaces)
//                {
//                    services.AddTransient(interfaceType, projectorType);
//                }
//            }
//        }
//
//        private static void AddEventHandlersAndConsumers(this IServiceCollection services, params Type[] markerTypes)
//        {
//            var eventHandlers = new List<Type>();
//            foreach (var type in markerTypes)
//            {
//                eventHandlers.AddRange(GetAllTypesImplementingOpenGenericType(typeof(IEventHandler<>), type.Assembly));
//            }
//
//            foreach (var handlerType in eventHandlers)
//            {
//                if (!handlerType.IsAbstract)
//                {
//                    var evtType = handlerType.BaseType.GetGenericArguments()[0];
//                    var consumerType = typeof(EventConsumer<>).MakeGenericType(evtType);
//                    var @interface = typeof(IEventHandler<>).MakeGenericType(evtType);
//
//                    services.AddTransient(@interface, handlerType);
//
//                    // TODO - there's got to be a better way to do this
//                    var method = typeof(ServiceCollectionHostedServiceExtensions).GetMethods()
//                        .Where(m => m.IsStatic && m.IsPublic)
//                        .First(m => m.Name == "AddHostedService")
//                        .MakeGenericMethod(consumerType);
//
//                    method.Invoke(null, new object[] { services });
//                }
//
//                //handlerType.GetInterfaces()
//                //.Where(t => t.GetGenericTypeDefinition() == typeof(IEventHandler<>))
//                //.Where(t => !t.IsAbstract)
//                //.ToList();
//
//                //foreach (var interfaceType in interfaces)
//                //{
//                //    services.AddTransient(interfaceType, handlerType);
//
//                //    var evtType = interfaceType.GetGenericArguments()[0];
//                //    var consumerType = typeof(EventConsumer<>).MakeGenericType(evtType);
//
//                //    // TODO - there's got to be a better way to do this
//                //    var method = typeof(ServiceCollectionHostedServiceExtensions).GetMethods()
//                //        .Where(m => m.IsStatic && m.IsPublic)
//                //        .First(m => m.Name == "AddHostedService")
//                //        .MakeGenericMethod(consumerType);
//
//                //    method.Invoke(null, new object[] { services });
//                // }
//            }
//        }
//
//        // Taken from stack overflow
//        private static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type openGenericType, Assembly assembly)
//        {
//            return from x in assembly.GetTypes()
//                   from z in x.GetInterfaces()
//                   let y = x.BaseType
//                   where
//                   (y != null && y.IsGenericType &&
//                   openGenericType.IsAssignableFrom(y.GetGenericTypeDefinition())) ||
//                   (z.IsGenericType &&
//                   openGenericType.IsAssignableFrom(z.GetGenericTypeDefinition()))
//                   select x;
//        }
    }
}
