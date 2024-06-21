using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTrinity.App.Core.Services
{
    public interface IMessageService
    {
        void Process(string msgType, string msgData);
    }
}
