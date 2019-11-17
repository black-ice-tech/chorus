using Chorus.CQRS;
using Chorus.DistributedLog.InMemory.Extensions;
using Chorus.Messaging;
using Chorus.Messaging.Abstractions;
using Chorus.Samples.RestApi.Appliers;
using Chorus.Samples.RestApi.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            services.AddHostedService<EventProjector<NumberAdded>>();
            services.AddHostedService<EventProjector<NumberMultiplied>>();
            services.AddHostedService<EventProjector<NumberSubtracted>>();

            services.AddTransient(typeof(IEventHandler<>), typeof(DemoEventHandler<>));

            services.AddTransient<IEventApplier<NumberAdded>, CurrentNumberInMemoryStoreApplier>();
            services.AddTransient<IEventApplier<NumberMultiplied>, CurrentNumberInMemoryStoreApplier>();
            services.AddTransient<IEventApplier<NumberSubtracted>, CurrentNumberInMemoryStoreApplier>();

            services.AddTransient<IEventApplier<NumberAdded>, CountAllNumbersOver100>();
            services.AddTransient<IEventApplier<NumberMultiplied>, CountAllNumbersOver100>();
            services.AddTransient<IEventApplier<NumberSubtracted>, CountAllNumbersOver100>();
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
