using System;
using System.Threading;
using System.Threading.Tasks;
using Chroniton;
using Chroniton.Jobs;
using Chroniton.Schedules;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartTrinityApi.AppConfig.Jobs;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityApi.Core.Interfaces.Services;
using SmartTrinityConsole.Infrastructure.Jobs;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityApi.AppConfig
{
    public class ApplicationStartup : IHostedService
    {
        ILogger<ApplicationStartup> _logger;

        IPumpService _pumpService;
        IServiceProcess _serviceProcess;
        ICommunicationManager _messageManager;

        ILogger<SocketReaderJob> _socketReaderlogger;
        ILogger<AbstractConnectionManager> _startConnectionlogger;
        Singularity singularity = Singularity.Instance;

        public ApplicationStartup(ILogger<ApplicationStartup> logger, ILogger<AbstractConnectionManager> startConnectionlogger, ILogger<SocketReaderJob> socketReaderlogger, ICommunicationManager messageManager, IServiceProcess serviceProcess, IPumpService pumpService)
        {
            _logger = logger;
            _pumpService = pumpService;
            _serviceProcess = serviceProcess;
            _messageManager = messageManager;

            _socketReaderlogger = socketReaderlogger;
            _startConnectionlogger = startConnectionlogger;

            singularity.Start();

            singularity.ScheduleJob(new EveryXTimeSchedule(TimeSpan.FromHours(1)),
            new ConnectionManagerJob(_startConnectionlogger, _messageManager, _pumpService), true);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting App..");

            singularity.ScheduleJob(new EveryXTimeSchedule(TimeSpan.FromMilliseconds(1)),
            new SocketReaderJob(_socketReaderlogger, _messageManager, _serviceProcess, _pumpService), true);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }



}