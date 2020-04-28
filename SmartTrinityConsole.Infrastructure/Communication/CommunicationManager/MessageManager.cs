using Microsoft.Extensions.Logging;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Infrastructure.Persistence;
using SmartTrinityConsole.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Security;
using System;

namespace SmartTrinityConsole.Infrastructure.CommunicationManager
{
    public class MessageManager : ICommunicationManager
    {
        private char _currentCrypt;
        private ILogger<ICommunicationManager> _logger;
        public int ILocalPort { get; private set; }
        private ICommunication _client = null;
        public bool IsConnected { get; private set; }
        public string MsgType { get; private set; }
        public string MsgData { get; private set; }
        public int ISeed { get; private set; }
        public int IKey { get; private set; }
        public long TimeoutEcho { get; private set; }
        public bool LogTryConnect { get; private set; }
        public IConnectionParams ConnectionParams { get; set; }

        public MessageManager(ILogger<ICommunicationManager> logger)
        {
            SetMessageParametersDefaultValues();
            _logger = logger;
        }
        public void SetMessageParametersDefaultValues()
        {
            ILocalPort = -1;
            _client = null;
            IsConnected = false;
            MsgType = null;
            MsgData = null;
            ISeed = Tools.GetSeeds() + 72;
            IKey = ISeed;
            _currentCrypt = '2';
            TimeoutEcho = 30000L;
            LogTryConnect = false;
        }

        public void Connect()
        {
            _client = ConnectionParams.BuildClient();
            _client._connectionTimeout = 20000;

            if (LogTryConnect)
            {
                _logger.LogDebug($"Trying to stablish connection with the host { ConnectionParams.ToString() }");
                LogTryConnect = false;
            }

            _client.Connect();
            ILocalPort = _client.GetLocalIdentifier();
            IsConnected = true;
            LogTryConnect = true;

            SmartSalePersistence.RemovePersistence();
            SmartPumpPersistence.RemovePersistence();
            SmartGradePersistence.RemovePersistence();

            StartConnection();
        }

        public char[] CryptMessage(char method, char[] inp, int inplen, int extraSize)
        {
            string msgWithPadLeft = Tools.LPad((inplen + extraSize).ToString(), "0", 5);
            string key;
            switch (method)
            {
                case '1':
                    key = $"{msgWithPadLeft}|1|{Tools.LPad(ILocalPort.ToString(), "0", 6)}";
                    return Tools.Encrypt(inp, inplen, key.ToCharArray(), key.Length);

                case '2':
                    key = $"{msgWithPadLeft}|2|{Tools.LPad(IKey.ToString(), "0", 6)}";
                    return Tools.Encrypt(inp, inplen, key.ToCharArray(), key.Length);
            }

            return inp;
        }

        public bool ClientIsConnected()
        {
            try { IsConnected = _client.IsConnected(); }
            catch (System.Exception) { IsConnected = false; }

            return IsConnected;
        }

        public void Disconnect()
        {
            _logger.LogDebug("Disconnecting from the server... ");
            try
            {
                _client.Disconnect();
                SetMessageParametersDefaultValues();
            }
            catch (Exception e)
            {
                SetMessageParametersDefaultValues();
                _logger.LogDebug($"Error while trying to disconnected from server | {e.Message}");
            }
            _logger.LogDebug("Disconnected from the server... ");
        }

        public void ReceiveSubscribedMessages()
        {
            try
            {
                MsgType = "";
                _client._receiveBufferSize = 8;
                string msgReceived = new string(_client.Receive());
                int bufferSize;
                int.TryParse(msgReceived.Substring(0, 5), out bufferSize);
                char tempCrypt = msgReceived[6];
                _client._receiveBufferSize = bufferSize;
                char[] aMsg = _client.Receive();

                aMsg = CryptMessage(tempCrypt, aMsg, bufferSize, 0);
                _client.LastReply = new string(CryptMessage(tempCrypt, _client.LastReply.ToCharArray(), bufferSize, 0));
                SetLastReplyToPersistence(_client.LastReply);

                string msg = new string(aMsg);
                string Smsg = msg.Substring(0, msg.Length - 1);

                string[] dataReceived = Smsg.Split('|');
                MsgType = dataReceived[1];
                MsgData = "";

                for (int i = 2; i < dataReceived.Length; i++)
                {
                    MsgData += $"|{dataReceived[i]}";
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Error while trying to subscribe a message | {e.Message}");
            }
        }

        public void SendMsg(string msgType, string eventType, string data)
        {
            try
            {
                if (!IsConnected) throw new Exception("Need to be connected before subscribing messages");

                string currentMsg = $"{msgType}|{eventType}|||{data}";

                if (!msgType.Equals("ECHO"))
                    _logger.LogDebug($"Message [{msgType}|{eventType}]");

                string msgWithPadLeft = Tools.LPad((currentMsg.Length + 1).ToString(), "0", 5);
                char[] msg = CryptMessage(_currentCrypt, currentMsg.ToCharArray(), currentMsg.Length, 1);

                if (!msgType.Equals("ECHO"))
                    _logger.LogDebug($"Sending message type {msgType} | {eventType} ...");

                _client.Send($"{msgWithPadLeft}|{_currentCrypt}|");
                _client.Send(msg);
                _client.Send("^".ToCharArray());
            }
            catch (Exception e)
            {
                throw new Exception($"Error while trying to send a message | {e.Message}");
            }
        }

        public void StartConnection()
        {
            if (_currentCrypt == '2')
            {
                IKey = ISeed;
                SendMsg("SESSION", ILocalPort.ToString(), "");
                IKey = ILocalPort;
            }
        }

        public void Subscribe(string subscribeType)
        {
            SendMsg("SUBSCRIBE", subscribeType, "");
        }

        public void SendSecureMsg(string eventType, string data)
        {
            // TODO: Create logic for send secure msg
            SendMsg("POST", eventType, data);
        }

        public void SendMsgWithResponse(string messageId, string msgType, string eventType, string data, IMessageReceptor messageReceptor)
        {
            // TODO: Create logic for send msg with response
            throw new NotImplementedException();
        }

        public bool SocketHasData()
        {
            try { return _client.SocketHasData(); }
            catch { return false; }
        }

        public void SetLastReplyToPersistence(string reply)
        {

            var data = reply.Split("|");
            if (data[1] == "RES_SECU_LOGIN" || data[1] == "RES_SECU_ACCESS_DENIED")
            {
                SmartUserPersistence.LastReply = reply;
            }
        }

        public string GetLastReply()
        {
            return _client.LastReply;
        }
    }
}