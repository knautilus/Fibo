using System;

namespace Fibo.Transport
{
    public interface IConsumer<T> : IDisposable
        where T : class
    {
        void SetAction(Handler<T> action);
    }
}
