using SmartTrinity.App.Core.Services;

namespace SmartTrinity.App.Api.Services
{
    public class TrinityBackgroundService : BackgroundService
    {
        private readonly ISmartTrinityService _smartTrinityService;

        public TrinityBackgroundService(ISmartTrinityService smartTrinityService)
        {
            _smartTrinityService = smartTrinityService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _smartTrinityService.Setup();
            await _smartTrinityService.HandleClientAsync();
        }
    }
}

