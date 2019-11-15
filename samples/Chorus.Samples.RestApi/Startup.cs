using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.InMemory.Extensions;
using Chorus.Messaging;
using Chorus.Messaging.Abstractions;
using Chorus.Samples.RestApi.Handlers;
using Chorus.Samples.RestApi.Projectors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Text;

namespace Chorus.Samples.RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddInMemoryDistributedLog();
            services.AddHostedService<EventConsumer<NumberAdded>>();
            services.AddHostedService<EventConsumer<NumberMultiplied>>();
            services.AddHostedService<EventConsumer<NumberSubtracted>>();

            services.AddHostedService<EventConsumer<PolicyStarted>>();
            services.AddHostedService<EventConsumer<PolicyIssued>>();
            services.AddHostedService<EventConsumer<PolicyEnded>>();
            services.AddHostedService<EventConsumer<PolicyCancelled>>();

            services.AddTransient(typeof(IEventHandler<>), typeof(DemoEventHandler<>));
            //services.AddTransient<IEventHandler<NumberAdded>, NumberAddedHandler>();
            //services.AddTransient<IEventHandler<NumberMultiplied>, NumberMultipliedHandler>();
            //services.AddTransient<IEventHandler<NumberSubtracted>, NumberSubtractedHandler>();

            services.AddTransient<IEventProjector<NumberAdded>, NumberInMemoryStoreProjector>();
            services.AddTransient<IEventProjector<NumberMultiplied>, NumberInMemoryStoreProjector>();
            services.AddTransient<IEventProjector<NumberSubtracted>, NumberInMemoryStoreProjector>();

            services.AddTransient<IEventProjector<PolicyStarted>, PolicyInMemoryStoreProjector>();
            services.AddTransient<IEventProjector<PolicyIssued>, PolicyInMemoryStoreProjector>();
            services.AddTransient<IEventProjector<PolicyEnded>, PolicyInMemoryStoreProjector>();
            services.AddTransient<IEventProjector<PolicyCancelled>, PolicyInMemoryStoreProjector>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
