using System;

namespace Fibo.Transport
{
    public class Response
    {
        public const int OkCode = 200;
        public const int ServerErrorCode = 500;

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
