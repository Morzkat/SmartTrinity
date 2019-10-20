using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SmartTrinityApi.Core.Interfaces;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Infrastructure.Tcp.Communication
{
    public class TcpCommunication : ICommunication
    {
        public Socket _socket { get; private set; }
        public IConnectionParams _params { get; set; }
        public int _recvBufferSize { get; set; }
        public int _timeout { get; set; }
        public int _connectionTimeout { get; set; }
        public int _bytesRead { get; set; }
        public string _lastReply { get; set; }
        public char[] _lastMsg { get; set; }

        public TcpCommunication()
        {
            _params = null;
            _connectionTimeout = 5000;
            _recvBufferSize = 65536;
        }

        public void Connect()
        {
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(_params.Host), _params.Port);
            _socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(serverAddress);
            //_socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.KeepAlive, 0);
        }

        public void Disconnect()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
        }

        public int GetLocalIdentifier()
        {
            return GetLocalPort();
        }

        public Stream GetInputStream()
        {
            throw new System.NotImplementedException();
        }

        public Stream GetOutputStream()
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, string> GetParams()
        {
            throw new System.NotImplementedException();
        }

        public bool IsConnected()
        {
            return _socket != null && _socket.Connected;
        }

        public char[] Recv()
        {
            int totalBytesRead = 0;
            _socket.ReceiveTimeout = 5000;
            byte[] byteArray = new byte[_recvBufferSize];
            char[] charArray = new char[totalBytesRead];

            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    try
                    {
                        if (totalBytesRead < 0)
                            totalBytesRead = 0;

                        int dataLength = _socket.Receive(byteArray, totalBytesRead, _recvBufferSize - totalBytesRead, SocketFlags.None);

                        if (dataLength < 0) throw new Exception("Socket is closed...");

                        totalBytesRead += dataLength;
                    }
                    catch (Exception e) { throw e; }

                    if (totalBytesRead >= _recvBufferSize) break;
                }
            }
            catch (Exception e2)
            {
                _bytesRead = totalBytesRead;
                charArray = new char[totalBytesRead];
                for (int i = 0; i < totalBytesRead; i++)
                { charArray[i] = (char)(byteArray[i] & 0xFF); }

                _lastReply = new string(charArray);

                throw e2;
            }

            _bytesRead = totalBytesRead;
            charArray = new char[totalBytesRead];
            for (int i = 0; i < totalBytesRead; i++)
            { charArray[i] = (char)(byteArray[i] & 0xFF); }

            _lastReply = new string(charArray);

            return charArray;
        }

        public void Send(string msg)
        {
            Send(msg.ToCharArray());
        }

        public void Send(char[] msg)
        {
            _lastMsg = msg;
            int msgLength = msg.Length;
            byte[] msgAux = new byte[msgLength];

            for (int i = 0; i < msgLength; i++)
            { msgAux[i] = (byte)msg[i]; }

            if (_socket == null) throw new Exception("Socket is null....");
            if (!IsConnected()) throw new Exception("Socket is disconnected....");

            int p = _socket.Send(msgAux, SocketFlags.None);

        }

        public void SetTimeout(int timeout)
        {
            try { _socket.SendTimeout = timeout; }
            catch (Exception e) { throw e; }

            _timeout = timeout;
        }

        public int GetLocalPort()
        {
            return ((IPEndPoint)_socket.LocalEndPoint).Port;
        }

        public void SetParams(IConnectionParams connectionParams)
        {
            _params = connectionParams;
        }

        // TODO: Implement logic for read from socket continuously. 
        public bool SocketHasData()
        {
            return _socket.Available > 0 ? true : false;
        }
    }
}