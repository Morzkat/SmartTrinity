namespace SmartTrinity.App.Core.Services
{
    public interface ISmartTrinityService
    {
        void Setup();
        void SetupRequestConfigurations();
        void SetupSubscriptionsToEvents();
    }
}
