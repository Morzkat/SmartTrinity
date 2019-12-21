using System.Collections.Generic;

namespace SmartTrinityConsole.Core.ServerResponse
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }

    public class ResponseWithList<T> : Response
    {
        public IEnumerable<T> List { get; set; }
    }

    public class ResponseWithElement<T> : Response
    {
        public T Element { get; set; }
    }
}