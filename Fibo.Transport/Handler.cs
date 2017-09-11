using System.Threading.Tasks;

namespace Fibo.Transport
{
    public delegate Task Handler<T>(T message, string sessionId)
        where T : class;
}
