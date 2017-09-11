using Fibo.Messages;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using NUnit.Framework;

namespace Fibo.Tests
{
    [TestFixture]
    public class RabbitSenderTests
    {
        private ISender<FibonacciMessage> _sender;

        [SetUp]
        public void SetUp()
        {
            _sender = new RabbitSender<FibonacciMessage>("rabbitmq://localhost", "guest", "guest");
        }

        [TearDown]
        public void TearDown()
        {
            if (_sender != null)
            {
                _sender.Dispose();
            }
        }

        [Test]
        public void ShouldStartRabbitSender()
        {
        }
    }
}
