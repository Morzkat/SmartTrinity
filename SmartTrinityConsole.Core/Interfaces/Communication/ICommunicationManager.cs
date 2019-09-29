using SmartTrinityApi.Core.Entities;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Interfaces.Communication
{
    public interface ICommunicationManager
    {
        //TODO: Create neccesary params and methods for the interface....
        int iLocalPort { get; }
        ICommunication client { get; }
        bool isConnected { get; }
        string msgType { get; }
        string msgData { get; }
        int iSeed { get; }
        int iKey { get; }
        char currentCrypt { get; }
        long timeoutEcho { get; }
        bool logTryConnect { get; }

        void Connect(IConnectionParams connectionParams);
        bool IsConnected();
        void Disconnected();
        char[] CryptMessage(char method, char[] inp, int inplen, int extraSize);
        void ReceiveSubscribedMessages();
        void SendMsg(string msgType, string eventType, string data);
        void Subscribe(string subscribeType);
        void StartConnection();
        void SetMessageData(string msgData);
        string GetMessageData();
        void GetMessageType();
        void GetTimeoutEcho();
        void SetTimeoutEcho(long timeoutEcho);

    }
}