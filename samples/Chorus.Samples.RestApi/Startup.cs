using System.IO;
using Chorus.CQRS;
using Chorus.DistributedLog;
using Chorus.DistributedLog.Abstractions;
using Chorus.DistributedLog.Extensions;
using Chorus.DistributedLog.TextFile.Extensions;
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
            services
                .AddChorus(typeof(NumberAdded))
                .AddTextFileDistributedLog(options =>
                {
                    options.StreamDirectory = Directory.GetCurrentDirectory();
                });
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
