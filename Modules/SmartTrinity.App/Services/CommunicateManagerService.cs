using Microsoft.Extensions.Logging;
using SmartTrinity.App.Core.Communication;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp.Extensions;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;

namespace SmartTrinity.App.Services
{
    public class CommunicateManagerService : ICommunicationManager
    {
        private char _currentCrypt;
        private ILogger<ICommunicationManager> _logger;
        public int ILocalPort { get; private set; }
        private ICommunication _client;
        public bool IsConnected { get; private set; }
        public string MsgType { get; private set; }
        public string MsgData { get; private set; }
        public int ISeed { get; private set; }
        public int IKey { get; private set; }
        public long TimeoutEcho { get; private set; }
        public bool LogTryConnect { get; private set; }
        public IConnectionParams ConnectionParams { get; set; }

        private string generation = "11345113451";
        private int generation2;
        private int generation3;

        public DateTime LastConnectionDate => throw new NotImplementedException();

        public CommunicateManagerService(ILogger<ICommunicationManager> logger)
        {
            _logger = logger;
            generation2 = GetGeneration(3) + 1;
            generation3 = GetGeneration(1);
            SetMessageParametersDefaultValues();
        }

        public void Connect()
        {
            _client = ConnectionParams.BuildClient();
            _client._connectionTimeout = 20000;

            if (LogTryConnect)
            {
                _logger.LogDebug($"Trying to stablish connection with the host {ConnectionParams.ToString()}");
                LogTryConnect = false;
            }

            _client.Connect();
            ILocalPort = _client.GetLocalIdentifier();
            IsConnected = true;
            LogTryConnect = true;

            StartConnection();
        }

        public void Disconnect()
        {
            _logger.LogDebug("Disconnecting from the server... ");
            try
            {
                _client.Disconnect();
                SetMessageParametersDefaultValues();
                IsConnected = false;
            }
            catch (Exception e)
            {
                SetMessageParametersDefaultValues();
                _logger.LogDebug($"Error while trying to disconnected from server | {e.Message}");
            }
            _logger.LogDebug("Disconnected from the server... ");
        }

        public bool ClientIsConnected()
        {
            try
            {
                IsConnected = _client.IsConnected();
            }
            catch (Exception) { IsConnected = false; }

            return IsConnected;
        }

        public char[] CryptMessage(char method, char[] inp, int inplen, int extraSize)
        {
            string msgWithPadLeft = (inplen + extraSize).ToString().LPad("0", 5);
            string key;

            switch (method)
            {
                case '1':
                    key = $"{msgWithPadLeft}|1|{ILocalPort.ToString().LPad("0", 6)}";
                    return inp.DecryptMessage(inplen, key.ToCharArray(), key.Length);

                case '2':
                    key = $"{msgWithPadLeft}|2|{IKey.ToString().LPad("0", 6)}";
                    return inp.DecryptMessage(inplen, key.ToCharArray(), key.Length);
            }

            return inp;
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
                if (!ClientIsConnected()) throw new Exception("Need to be connected before subscribing messages");

                string currentMsg = $"{msgType}|{eventType}|||{data}";

                if (!msgType.Equals("ECHO"))
                    _logger.LogDebug($"Message [{msgType}|{eventType}]");

                string msgWithPadLeft = (currentMsg.Length + 1).ToString().LPad("0", 5);
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

        public void SendSecureMsg(string eventType, string data)
        {
            SendMsg("POST", eventType, data);
        }

        public void SendMsgWithResponse(string messageId, string msgType, string eventType, string data, IMessageReceptor messageReceptor)
        {
            // TODO: Create logic for send msg with response
            throw new NotImplementedException();
        }

        public void Subscribe(string subscribeType)
        {
            SendMsg("SUBSCRIBE", subscribeType, "");
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

        public bool SocketHasData()
        {
            try { return _client.SocketHasData(); }
            catch { return false; }
        }

        public string GetLastReply()
        {
            return _client.LastReply;
        }

        private void SetMessageParametersDefaultValues()
        {
            ILocalPort = -1;
            _client = null;
            IsConnected = false;
            MsgType = null;
            MsgData = null;
            ISeed = GetSeeds() + 72;
            IKey = ISeed;
            _currentCrypt = '2';
            TimeoutEcho = 30000L;
            LogTryConnect = false;
        }

        private int GetGeneration(int generation31)
        {
            generation3 = generation31;
            return generation31 * 3;
        }

        public int GetSeeds()
        {
            return generation3 * generation2 * generation2 * generation2 * generation2 * generation2 + generation.Length * generation2 * generation2;
        }
    }
}
