using MassTransit;
using System;
using System.Threading.Tasks;

namespace Fibo.Transport.Rabbit
{
    public class RabbitSender<T> : ISender<T>
        where T : class
    {
        private readonly IBusControl _bus;

        public RabbitSender(string hostUri, string username, string password)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(bc =>
            {
                var host = bc.Host(new Uri(hostUri), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
            });

            _bus.Start();
        }

        public async Task<Response> SendAsync(T message, string sessionId)
        {
            try
            {
                await _bus.Publish(message, c => c.Headers.Set(Constants.SessionIdHeader, sessionId));
                return new Response { StatusCode = Response.OkCode, Message = "Message sent" };
            }
            catch (Exception ex)
            {
                return new Response { StatusCode = Response.ServerErrorCode, Message = ex.Message, Exception = ex };
            }
        }

        public void Dispose()
        {
            _bus.Stop();
        }
    }
}
