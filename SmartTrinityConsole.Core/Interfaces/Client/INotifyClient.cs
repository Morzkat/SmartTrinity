using System.Collections.Generic;

namespace SmartTrinityConsole.Core.Entities.Client
{
      public interface INotifyClient<T>
      {
            string EndPoint { get; }
            T GetClientData();
      }
}
