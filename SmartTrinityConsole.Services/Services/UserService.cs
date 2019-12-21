
using SmartTrinityApi.Core.Interfaces.Process;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services
{
    public class UserService : IUserService
    {

        ICommunicationManager _messageManager;

        public UserService(ICommunicationManager messageManager)
        {
            _messageManager = messageManager;
        }

        public void LogInUser()
        {
            if (!SmartUserPersistence.UserIsLogged)
            {
                string dataForLogin = SmartUserPersistence.PrepareDataForLogin();
                _messageManager.SendMsg("POST", "REQ_SECU_LOGIN", dataForLogin);
                SmartUserPersistence.UserIsLogged = true;
            }
        }
    }
}















