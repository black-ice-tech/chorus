using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Chorus.CQRS;
using Chorus.DistributedLog.Abstractions;
using Chorus.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Chorus.DistributedLog.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChorus(this IServiceCollection services, params Type[] markerTypes)
        {
            services.AddSingleton<ITopicNamingConvention, KebabCaseTopicNamingConvention>();
            services.AddTransient<IStreamConsumer, StreamConsumer>();

            var eventTypes = GetAllTypesImplementingInterface<IEvent>(markerTypes).ToList();

            services.AddServicesAsTransient(typeof(IEventHandler<>), eventTypes, markerTypes);
            services.AddServicesAsTransient(typeof(IEventApplier<>), eventTypes, markerTypes);

            services.AddEventConsumers(eventTypes);
            services.AddEventProjectors(eventTypes);
            return services;
        }

        private static IServiceCollection AddEventProjector<TEvent>(this IServiceCollection services)
            where TEvent : IEvent
        {
            services.AddHostedService<EventProjector<TEvent>>();
            return services;
        }

        private static IServiceCollection AddEventConsumer<TEvent>(this IServiceCollection services)
            where TEvent : IEvent
        {
            services.AddHostedService<EventConsumer<TEvent>>();
            return services;
        }

        private static void AddEventConsumers(this IServiceCollection services, IEnumerable<Type> eventTypes)
        {
            foreach (var eventType in eventTypes)
            {
                var method = typeof(ServiceCollectionExtensions)
                    .GetMethod(nameof(AddEventConsumer), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(eventType);
                method?.Invoke(null, new object[] {services});
            }
        }

        private static void AddEventProjectors(this IServiceCollection services, IEnumerable<Type> eventTypes)
        {
            foreach (var eventType in eventTypes)
            {
                var method = typeof(ServiceCollectionExtensions)
                    .GetMethod(nameof(AddEventProjector), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(eventType);
                method?.Invoke(null, new object[] {services});
            }
        }

        private static void AddServicesAsTransient(this IServiceCollection services, Type openGenericType,
            IEnumerable<Type> eventTypes, params Type[] markerTypes)
        {
            foreach (var eventType in eventTypes)
            {
                var eventHandlerTypes = GetAllTypesImplementingOpenGenericType(eventType, openGenericType, markerTypes);

                foreach (var eventHandlerType in eventHandlerTypes)
                {
                    var interfaceType = openGenericType.MakeGenericType(eventType);
                    services.AddTransient(interfaceType, eventHandlerType);
                }
            }
        }

        private static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type genericTypeParameter, Type openGenericType,
            Type[] markerTypes)
        {
            var genericType = openGenericType.MakeGenericType(genericTypeParameter);

            var implementationTypes = markerTypes
                .SelectMany(type => type.Assembly.GetTypes())
                .Where(type => !type.IsAbstract)
                .Where(type => !type.IsInterface)
                .Where(type => genericType.IsAssignableFrom(type));

            return implementationTypes;
        }

        private static IEnumerable<Type> GetAllTypesImplementingInterface<TInterface>(params Type[] markerTypes)
        {
            var allImplementingTypes = markerTypes
                .SelectMany(type => type.Assembly.GetTypes())
                .Where(t => typeof(TInterface).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Distinct();

            return allImplementingTypes;
        }
    }
}
