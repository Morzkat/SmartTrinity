using SmartTrinityApi.Core.Entities;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Interfaces.Communication
{
    public interface ICommunicationManager
    {
        //TODO: Create neccesary params and methods for the interface....
        int _iLocalPort { get; }
        ICommunication _client { get; }
        bool _isConnected { get; }
        string _msgType { get; }
        string _msgData { get; }
        int _iSeed { get; }
        int _iKey { get; }
        char _currentCrypt { get; }
        long _timeoutEcho { get; }
        bool _logTryConnect { get; }
        IConnectionParams _connectionParams { get; }

        void Connect();
        bool IsConnected();
        void Disconnect();
        char[] CryptMessage(char method, char[] inp, int inplen, int extraSize);
        void ReceiveSubscribedMessages();
        void SendMsg(string msgType, string eventType, string data);
        void SendMsgSecure(string msg, string data);
        void Subscribe(string subscribeType);
        void StartConnection();
        void SetMessageData(string msgData);
        string GetMessageData();
        string GetMessageType();
        void SetMessageType(string msgType);
        long GetTimeoutEcho();
        void SetTimeoutEcho(long timeoutEcho);
        void SetConnectionParams(IConnectionParams connectionParams);

    }
}