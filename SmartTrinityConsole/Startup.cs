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
            
            services.AddSingleton<ICommunicationManager, MessageManager>();
            services.AddScoped<ISalesServices, SalesService>();
            services.AddScoped<IServiceProcess, ServiceProcess>();
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

            app.UseRouting();
            app.UseEndpoints( endpoints => 
            {
                endpoints.MapControllers();
            });
        }
    }
}
