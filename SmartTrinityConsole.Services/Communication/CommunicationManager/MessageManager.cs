using Microsoft.Extensions.Logging;
using SmartTrinityApi.Common;
using SmartTrinityApi.Core.Interfaces.Communication;
using SmartTrinityConsole.Interfaces.Communication;
using System;

namespace SmartTrinityConsole.Services.CommunicationManager
{
    // TODO: Create interface for the message manager
    public class MessageManager : ICommunicationManager
    {
        // Implement all the communication here....
        public int _iLocalPort { get; private set; }
        public ICommunication _client { get; private set; }
        public bool _isConnected { get; private set; }
        public string _msgType { get; private set; }
        public string _msgData { get; private set; }
        public int _iSeed { get; private set; }
        public int _iKey { get; private set; }
        public char _currentCrypt { get; private set; }
        public long _timeoutEcho { get; private set; }
        public bool _logTryConnect { get; private set; }
        private ILogger<ICommunicationManager> _logger;
        public IConnectionParams _connectionParams { get; private set; }

        public MessageManager(ILogger<ICommunicationManager> logger)
        {
            _iLocalPort = -1;
            _client = null;
            _isConnected = false;
            _msgType = null;
            _msgData = null;
            _iSeed = Tools.GetSeeds() + 72; // TODO: Get seed from tools (create method and logic for get seed )
            _iKey = _iSeed;
            _currentCrypt = '2';
            _timeoutEcho = 30000L;
            _logTryConnect = false;
            _logger = logger;
        }

        public void Connect()
        {
            _client = _connectionParams.BuildClient();
            _client._connectionTimeout = 20000;

            if (_logTryConnect)
            {
                _logger.LogDebug($"Trying to stablish connection with the host { _connectionParams.ToString() }");
                _logTryConnect = false;
            }

            _client.Connect();
            _iLocalPort = _client.GetLocalIdentifier();
            _isConnected = true;
            _logTryConnect = true;
            // TODO: Remove this isn't necessary
            StartConnection();
        }

        public char[] CryptMessage(char method, char[] inp, int inplen, int extraSize)
        {
            // $"{inplen + extraSize}".PadLeft(5, '0')
            string msgWithPadLeft = Tools.Lpad((inplen + extraSize).ToString(), "0", 5);
            string key;
            switch (method)
            {
                case '1':
                    key = $"{msgWithPadLeft}|1|{Tools.Lpad(_iLocalPort.ToString(), "0", 6)}";
                    return Tools.EncryptMessage(inp, inplen, key.ToCharArray(), key.Length);

                case '2':
                    key = $"{msgWithPadLeft}|2|{Tools.Lpad(_iKey.ToString(), "0", 6)}";
                    return Tools.EncryptMessage(inp, inplen, key.ToCharArray(), key.Length);
            }

            return inp;
        }

        public bool IsConnected()
        {
            return _client.IsConnected();
        }

        public void Disconnect()
        {
            _logger.LogDebug("Disconnecting from the server... ");

            try
            {
                _client.Disconnect();
                _logger.LogDebug("Disconnected from the server... ");
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public void ReceiveSubscribedMessages()
        {
            throw new System.NotImplementedException();
        }

        public void SendMsg(string msgType, string eventType, string data)
        {
            if (!_isConnected) throw new Exception("Need to be connected before subscribing messages");

            string currentMsg = $"{msgType}|{eventType}|||{data}";

            if (!msgType.Equals("ECHO"))
                _logger.LogDebug($"Message [{currentMsg}]");

            string msgWithPadLeft = Tools.Lpad((currentMsg.Length + 1).ToString(),"0", 5);
            char[] msg = CryptMessage(_currentCrypt, currentMsg.ToCharArray(), currentMsg.Length, 1);

            if (!msgType.Equals("ECHO"))
                _logger.LogDebug($"Sending message type {msgType} | {eventType} ...");

            _logger.LogDebug("-----------------------------------------------------------Crypt Message-----------------------------------------------------------------------------");
            _logger.LogDebug($"message: { new string(msg) }");
            _logger.LogDebug("-----------------------------------------------------------Crypt Message-----------------------------------------------------------------------------");

            _client.Send($"{msgWithPadLeft}|{_currentCrypt}|");
            _client.Send(msg);
            _client.Send("^".ToCharArray());
        }

        public void SendMsgSecure(string msg, string data)
        {
            try
            {
                SendMsg("POST", msg, data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetMessageType()
        {
            return _msgType;
        }

        public void SetMessageType(string msgType)
        {
            _msgType = msgType;
        }

        public string GetMessageData()
        {
            return _msgData;
        }

        public void SetMessageData(string msgData)
        {
            _msgData = msgData;
        }

        public long GetTimeoutEcho()
        {
            return _timeoutEcho;
        }

        public void SetTimeoutEcho(long timeoutEcho)
        {
            _timeoutEcho = timeoutEcho;
        }

        public void StartConnection()
        {
            if (_currentCrypt == '2')
            {
                _iKey = _iSeed;
                SendMsg("SESSION", _iLocalPort.ToString(), "");
                _iKey = _iLocalPort;
            }
        }

        public void Subscribe(string subscribeType)
        {
            SendMsg("SUBSCRIBE", subscribeType, "");
        }

        public void SetConnectionParams(IConnectionParams connectionParams)
        {
            _connectionParams = connectionParams;
        }
    }
}