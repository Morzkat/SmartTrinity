using System;
using System.Threading.Tasks;
using Chroniton;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Jobs;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityApi.AppConfig.Jobs
{
    public class SocketReaderJob : AbstractSocketReaderJob, IJob, IJobBase
    {
        public string Name { get; set; }
        public virtual ScheduleMissedBehavior ScheduleMissedBehavior { get; set; }

        public SocketReaderJob(ILogger<AbstractSocketReaderJob> logger, ICommunicationManager messageManager,
         IServiceProcess serviceProcess, IPumpService pumpService) : base(logger, messageManager, serviceProcess, pumpService)
        {
            SmartPumpPersistence.LastUpdate = DateTime.Now;
        }

        public Task Start(DateTime scheduledTime)
        {
            ReadFromSocket();
            return Task.CompletedTask;
        }
    }
}