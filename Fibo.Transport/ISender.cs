using System;
using System.Threading.Tasks;

namespace Fibo.Transport
{
    public interface ISender<T> : IDisposable
        where T : class
    {
        Task<Response> SendAsync(T message, string sessionId);
    }
}
