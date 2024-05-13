using System.Net;
using System.Net.Sockets;
using SmartTrinity.App.Core.Communication.Adapaters.Tcp;
using SmartTrinity.App.Core.Communication.Adapters.Tcp;

namespace SmartTrinity.App.Infrastructure.Communication.Adapaters.Tcp
{
    public class TcpCommunication : ITcpCommunication
    {
        public Socket _socket { get; private set; }
        public IConnectionParams _params { get; set; }
        public int _receiveBufferSize { get; set; }
        public int _timeout { get; set; }
        public int _connectionTimeout { get; set; }
        public int _bytesRead { get; set; }
        public string LastReply { get; set; }
        public char[] LastMsg { get; set; }

        public TcpCommunication()
        {
            _params = null;
            _connectionTimeout = 5000;
            _receiveBufferSize = 65536;
        }

        public void Connect()
        {
            IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(_params.Host), _params.Port);
            _socket = new Socket(serverAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(serverAddress);
            // Tools.CurrentSocketPort = ((IPEndPoint)_socket.LocalEndPoint).Port;
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
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetParams()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            return _socket != null && _socket.Connected;
        }

        public char[] Receive()
        {
            int totalBytesRead = 0;
            _socket.ReceiveTimeout = 5000;
            byte[] byteArray = new byte[_receiveBufferSize];
            char[] charArray = new char[totalBytesRead];

            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    try
                    {
                        if (totalBytesRead < 0)
                            totalBytesRead = 0;

                        int dataLength = _socket.Receive(byteArray, totalBytesRead, _receiveBufferSize - totalBytesRead, SocketFlags.None);

                        if (dataLength < 0) throw new Exception("Socket is closed...");

                        totalBytesRead += dataLength;
                    }
                    catch (Exception e) { throw e; }

                    if (totalBytesRead >= _receiveBufferSize) break;
                }
            }
            catch (Exception e2)
            {
                _bytesRead = totalBytesRead;
                charArray = new char[totalBytesRead];
                for (int i = 0; i < totalBytesRead; i++)
                { charArray[i] = (char)(byteArray[i] & 0xFF); }

                LastReply = new string(charArray);

                throw e2;
            }

            _bytesRead = totalBytesRead;
            charArray = new char[totalBytesRead];
            for (int i = 0; i < totalBytesRead; i++)
            { charArray[i] = (char)(byteArray[i] & 0xFF); }

            LastReply = new string(charArray);

            return charArray;
        }

        public void Send(string msg)
        {
            Send(msg.ToCharArray());
        }

        public void Send(char[] msg)
        {
            LastMsg = msg;
            int msgLength = msg.Length;
            byte[] msgAux = new byte[msgLength];

            for (int i = 0; i < msgLength; i++)
            { msgAux[i] = (byte)msg[i]; }

            if (_socket == null) throw new Exception("Socket is null....");
            if (!IsConnected()) throw new Exception("Socket is disconnected....");

            _socket.Send(msgAux, SocketFlags.None);
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
            try { return _socket.Available > 0 ? true : false; }
            catch { return false; }
        }
    }
}