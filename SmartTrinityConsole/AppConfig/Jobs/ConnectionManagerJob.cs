using System;
using System.Threading.Tasks;
using Chroniton;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Jobs;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityApi.AppConfig.Jobs
{
    public class ConnectionManagerJob : AbstractConnectionManager, IJob, IJobBase
    {
        public string Name { get; set; }
        public virtual ScheduleMissedBehavior ScheduleMissedBehavior { get; set; }

        public ConnectionManagerJob(ILogger<AbstractConnectionManager> logger, ICommunicationManager messageManager,
        IPumpService pumpService) : base(logger, messageManager, pumpService)
        {
            SmartPumpPersistence.LastUpdate = DateTime.Now;
        }

        public Task Start(DateTime scheduledTime)
        {
            _logger.LogDebug("Starting job..");
            CheckConnectionStatus();
            return Task.CompletedTask;
        }
    }
}