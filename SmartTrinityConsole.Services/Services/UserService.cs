
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

        // HACK: Look for way to refactor this code...
        public bool UserIsConnected(string reply)
        {
            var data = reply.Split("|");
            if (data[1] == "RES_SECU_LOGIN")
            {
                var response = data[4].Split("=")[1];
                if (response == "DENIED")
                {
                    SmartUserPersistence.UserIsLogged =  false;
                    return false;
                }
            } else if (data[1] == "RES_SECU_ACCESS_DENIED")
            {
                var response = data[6].Split("=")[1];
                if (response == "DENIED")
                {
                    SmartUserPersistence.UserIsLogged =  false;
                    return false;
                }
            }
            return true;
        }
    }
}
















