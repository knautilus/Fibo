using System;
using System.Threading.Tasks;
using MassTransit;

namespace Fibo.Transport.Rabbit
{
    public class RabbitConsumer<T> : IConsumer<T>
        where T : class
    {
        private Handler<T> _action;
        private readonly IBusControl _bus;

        public RabbitConsumer(string hostUri, string username, string password, string queueName)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(bc =>
            {
                var host = bc.Host(new Uri(hostUri), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                bc.ReceiveEndpoint(host, queueName, ep =>
                {
                    ep.PurgeOnStartup = true;
                    ep.Handler<T>(context =>
                    {
                        var sessionId = context.Headers.Get(Constants.SessionIdHeader, string.Empty);
                        return ConsumeAsync(context.Message, sessionId);
                    });
                });
            });
        }

        public void SetAction(Handler<T> action)
        {
            _action = action;
            _bus.Start();
        }

        public void Dispose()
        {
            _bus.Stop();
        }

        private Task ConsumeAsync(T message, string sessionId)
        {
            return _action?.Invoke(message, sessionId);
        }
    }
}
