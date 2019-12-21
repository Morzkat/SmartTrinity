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
using SmartTrinityApi.AppConfig;
using Chroniton;
using SmartTrinityApi.Core.Interfaces.UnitOfWork;
using SmartTrinityConsole.Infrastructure.Database.UnitOfWork;
using SmartTrinityConsole.Infrastructure.Database.Config;

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
            services.AddSingleton<IPumpService, PumpService>();
            // services.AddSingleton<IMainService, MainService>();
            //Process
            services.AddTransient<IPumpProcess, PumpProcess>();
            services.AddTransient<IServiceProcess, ServiceProcess>();
            //ComunicationManager
            services.AddSingleton<ICommunicationManager, MessageManager>();
            
            services.AddSingleton<ISingularity, Singularity>(serviceProvider => Singularity.Instance);
            
            services.AddTransient<IUserService, UserService>();

            // Task
            services.AddHostedService<ApplicationStartup>();

            // UnitOfWork and Repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();


            // Dapper configuration
            DapperConfigurations.ConfigureDapper();
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
