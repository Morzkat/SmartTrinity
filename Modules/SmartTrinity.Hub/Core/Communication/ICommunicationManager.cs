using SmartTrinity.App.Core.Communication.Adapters.Tcp;

namespace SmartTrinity.App.Core.Communication
{
    public interface ICommunicationManager
    {
        int ILocalPort { get; }
        bool IsConnected { get; }
        string MsgType { get; }
        string MsgData { get; }
        int ISeed { get; }
        int IKey { get; }
        long TimeoutEcho { get; }
        bool LogTryConnect { get; }
        IConnectionParams ConnectionParams { get; set; }
        DateTime LastConnectionDate { get; }

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
        bool SocketHasData();
        string GetLastReply();
    }
}