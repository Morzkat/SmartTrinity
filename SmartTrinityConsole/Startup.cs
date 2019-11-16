using Microsoft.AspNetCore.Builder;
using SmartTrinityConsole.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using SmartTrinityApi.Core.Interfaces.Process;
using Microsoft.Extensions.DependencyInjection;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.CommunicationManager;
using SmartTrinityApi.Infrastructure.Hubs;
using SmartTrinityApi.Services.Process;
using SmartTrinityConsole.Services.Process;

namespace SmartTrinityApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddCors(options =>
           {
               options.AddPolicy("CorsPolicy",
                builder => builder.WithOrigins(Configuration.GetSection("ClientHost").Value)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
           });

            //Services
            services.AddMvc();
            services.AddScoped<ISalesServices, SalesService>();
            services.AddSingleton<IPumpService, PumpService>();
            // services.AddSingleton<IMainService, MainService>();
            //Process
            services.AddTransient<IPumpProcess, PumpProcess>();
            services.AddTransient<IServiceProcess, ServiceProcess>();
            //ComunicationManager
            services.AddSingleton<ICommunicationManager, MessageManager>();
            // Task
            services.AddHostedService<LongTimeTask>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();


            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
               endpoints.MapHub<SmartPumpHub>("/Hub/SmartPump");
           });
        }
    }
}
