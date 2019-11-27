using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Communication
{
    public interface ICommunication
    {
        Socket _socket { get; }
        IConnectionParams _params { get; set; }
        int _recvBufferSize { get; set; }
        int _timeout { get; set; }
        int _connectionTimeout { get; set; }
        int _bytesRead { get; set; }
        string _lastReply { get; set; }
        char[] _lastMsg { get; set; }

        void Connect();

        void Disconnect();

        bool IsConnected();

        int GetLocalIdentifier();

        void Send(String msg);

        void Send(char[] msg);

        char[] Recv();

        Stream GetInputStream();

        Stream GetOutputStream();

        void SetTimeout(int timeout);

        int GetLocalPort();

        void SetParams(IConnectionParams connectionParams);

        Dictionary<string, string> GetParams();

        bool SocketHasData();
    }
}
