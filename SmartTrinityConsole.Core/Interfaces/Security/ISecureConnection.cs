namespace SmartTrinityConsole.Interfaces.Security 
{
    public interface ISecureConnection 
    {
        void SendMsg(string paramstring1, string paramstring2, string paramstring3);

        //TODO: change parameters name
        void SendMsgWithResponse(string paramstring1, string paramstring2, string paramstring3, string paramstring4,
                             IMessageReceptor messageReceptor);

        IUser getUser();
    }
}