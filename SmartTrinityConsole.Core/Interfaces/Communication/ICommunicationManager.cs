using SmartTrinityApi.Core.Entities;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Security;

namespace SmartTrinityConsole.Interfaces.Communication
{
    public interface ICommunicationManager
    {
        int ILocalPort { get; }
        ICommunication Client { get; }
        bool IsConnected { get; }
        string MsgType { get; }
        string MsgData { get; }
        int ISeed { get; }
        int IKey { get; }
        long TimeoutEcho { get; }
        bool LogTryConnect { get; }
        IConnectionParams ConnectionParams { get; set; }
        // IUser GetUser();

        void Connect();
        void Disconnect();
        bool ClientIsConnected();
        char[] CryptMessage(char method, char[] inp, int inplen, int extraSize);
        void ReceiveSubscribedMessages();
        void SendMsg(string msgType, string eventType, string data);
        void SendSecureMsg(string eventType, string data);
        void SendMsgWithResponse(string messageId, string msgType, string eventType, string data, IMessageReceptor messageReceptor);
        void Subscribe(string subscribeType);
        void StartConnection();
    }
}