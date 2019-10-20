using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SmartTrinityConsole.Services.Sales;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.CommunicationManager;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityConsole.Infrastructure.Process;
using SmartTrinityApi.Infrastructure.Process;
using SmartTrinityConsole.Services.Pump;
using SmartTrinityConsole.Services;
using SmartTrinityApi.Controllers;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
           {
               endpoints.MapControllers();
               endpoints.MapHub<PumpSalesHub>("PumpSalesHub");
           });
        }
    }
}
