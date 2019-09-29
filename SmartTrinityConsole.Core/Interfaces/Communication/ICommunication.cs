using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmartTrinityApi.Core.Interfaces.Communication
{
    public interface ICommunication
    {
        void Connect();

        void SetConnectionParams(IConnectionParams connectionParams);

        void Disconnect();

        bool IsConnected();

        int GetLocalIdentifier();

        void SetTimeout(int time);

        void SetConnnectionTimeout(int time);

        void SetRecvBufferSize(int bufferSize);

        void Send(String msg);

        void Send(char[] msg);

        char[] Recv();

        Stream GetInputStream();

        Stream GetOutputStream();

        Dictionary<string, string> GetParams();
    }
}
