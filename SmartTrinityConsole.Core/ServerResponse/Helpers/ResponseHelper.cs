using System.Collections.Generic;
using SmartTrinityConsole.Core.ServerResponse;

namespace SmartTrinityApi.Core.ServerResponse.Helpers
{
    public static class ResponseHelper
    {
        public static Response NewResponse(string message = "", string error = "", bool success = false)
        {
            return new Response
            {
                Error = error,
                Success = success,
                Message = message
            };
        }

        public static ResponseWithElement<T> NewResponseWithElement<T>(T element, string message = "", string error = "", bool success = false)
        {
            return new ResponseWithElement<T>
            {
                Error = error,
                Success = success,
                Element = element,
                Message = message
            };
        }

        public static ResponseWithList<T> NewResponseList<T>(IEnumerable<T> elements, string message = "", string error = "", bool success = false)
        {
            return new ResponseWithList<T>
            {
                Error = error,
                List = elements,
                Success = success,
                Message = message
            };
        }
    }
}
