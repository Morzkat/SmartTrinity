using System.Collections.Generic;

namespace SmartTrinityConsole.Core.Entities.ServerResponse
{
      public class Result<T>
      {
            public T Response { get; set; }
            public int StatusCode { get; set; }
      }

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

      public enum StatusCode
      {
            Ok = 200,
            Created = 201,
            Accepted = 202,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            MethodNotAllowed = 405,
            Conflict = 409,
            TooEarly = 425,
            InternalServerError = 500,
            BadGateway = 501,
            ServiceUnavailable = 503,
            HTTPVersionNotSupported = 505
      }
}