using System.Collections.Generic;
using System.IO;
using SmartTrinityApi.Core.Interfaces;
using SmartTrinityApi.Core.Interfaces.Communication;

namespace SmartTrinityConsole.Services.Tcp.Communication
{
    public class TcpCommunication : ICommunication
    {
        public void Connect()
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        public Stream getInputStream()
        {
            throw new System.NotImplementedException();
        }

        public Stream GetInputStream()
        {
            throw new System.NotImplementedException();
        }

        public int GetLocalIdentifier()
        {
            throw new System.NotImplementedException();
        }

        public Stream getOutputStream()
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
            throw new System.NotImplementedException();
        }

        public char[] Recv()
        {
            throw new System.NotImplementedException();
        }

        public void Send(string msg)
        {
            throw new System.NotImplementedException();
        }

        public void Send(char[] msg)
        {
            throw new System.NotImplementedException();
        }

        public void SetConnectionParams(IConnectionParams connectionParams)
        {
            throw new System.NotImplementedException();
        }

        public void SetConnnectionTimeout(int time)
        {
            throw new System.NotImplementedException();
        }

        public void SetRecvBufferSize(int bufferSize)
        {
            throw new System.NotImplementedException();
        }

        public void SetTimeout(int time)
        {
            throw new System.NotImplementedException();
        }
    }
}