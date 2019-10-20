using SmartTrinityApi.Hubs;
using Microsoft.AspNetCore.Builder;
using SmartTrinityConsole.Services;
using SmartTrinityConsole.Services.Pump;
using SmartTrinityConsole.Services.Sales;
using Microsoft.Extensions.Configuration;
using SmartTrinityApi.Infrastructure.Process;
using SmartTrinityApi.Core.Interfaces.Process;
using Microsoft.Extensions.DependencyInjection;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Process;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.CommunicationManager;
using Microsoft.AspNetCore.Hosting;

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
            services.AddMvc();
            services.AddSignalR();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                 builder => builder.WithOrigins("http://localhost:4200")
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials());
            });

            //Services
            services.AddScoped<ISalesServices, SalesService>();
            services.AddSingleton<IPumpService, PumpService>();
            services.AddSingleton<IMainService, MainService>();
            //Process
            services.AddTransient<IPumpProcess, PumpProcess>();
            services.AddTransient<IServiceProcess, ServiceProcess>();
            //ComunicationManager
            services.AddSingleton<ICommunicationManager, MessageManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }*/

            app.UseDeveloperExceptionPage();
            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
               endpoints.MapHub<SmartPumpHub>("/SmartPump");
           });
        }
    }
}
