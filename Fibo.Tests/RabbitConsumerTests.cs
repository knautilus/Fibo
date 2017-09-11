using Fibo.Messages;
using Fibo.Transport;
using Fibo.Transport.Rabbit;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fibo.Tests
{
    [TestFixture]
    public class RabbitConsumerTests
    {
        private IConsumer<FibonacciMessage> _consumer;

        [SetUp]
        public void SetUp()
        {
            _consumer = new RabbitConsumer<FibonacciMessage>("rabbitmq://localhost", "guest", "guest", "testqueue");
        }

        [TearDown]
        public void TearDown()
        {
            if (_consumer != null)
            {
                _consumer.Dispose();
            }
        }

        [Test]
        public void ShouldStartRabbitConsumer()
        {
            _consumer.SetAction((x, y) => { return Task.CompletedTask; });
        }
    }
}
