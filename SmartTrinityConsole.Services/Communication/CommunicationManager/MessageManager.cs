using SmartTrinityApi.Core.Entities;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Communication;

namespace SmartTrinityConsole.Services.CommunicationManager
{
    // TODO: Create interface for the message manager
    public class MessageManager : ICommunicationManager
    {
        // Implement all the communication here....
        public int iLocalPort { get; private set; }
        public ICommunication client { get; private set; }
        public bool isConnected { get; private set; }
        public string msgType { get; private set; }
        public string msgData { get; private set; }
        public int iSeed { get; private set; }
        public int iKey { get; private set; }
        public char currentCrypt { get; private set; }
        public long timeoutEcho { get; private set; }
        public bool logTryConnect { get; private set; }

        public MessageManager () 
        {
            iLocalPort = -1;
            client = null;
            isConnected = false;
            msgType = null;
            msgData = null;
            iSeed = 0; // TODO: Get seed from tools (create method and logic for get seed )
            iKey = iSeed;
            currentCrypt = '2';
            timeoutEcho = 30000L;
            logTryConnect = false;
        }

        public void Connect(IConnectionParams connectionParams)
        {
            client = connectionParams.BuildClient();
        }

        public char[] CryptMessage(char method, char[] inp, int inplen, int extraSize)
        {
            throw new System.NotImplementedException();
        }

        public void Disconnected()
        {
            throw new System.NotImplementedException();
        }

        public string GetMessageData()
        {
            throw new System.NotImplementedException();
        }

        public void GetMessageType()
        {
            throw new System.NotImplementedException();
        }

        public void GetTimeoutEcho()
        {
            throw new System.NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new System.NotImplementedException();
        }

        public void ReceiveSubscribedMessages()
        {
            throw new System.NotImplementedException();
        }

        public void SendMsg(string msgType, string eventType, string data)
        {
            throw new System.NotImplementedException();
        }

        public void SetMessageData(string msgData)
        {
            throw new System.NotImplementedException();
        }

        public void SetTimeoutEcho(long timeoutEcho)
        {
            throw new System.NotImplementedException();
        }

        public void StartConnection()
        {
            throw new System.NotImplementedException();
        }

        public void Subscribe(string subscribeType)
        {
            throw new System.NotImplementedException();
        }
    }
}