using System.Numerics;
using System.Threading.Tasks;
using Fibo.Calculator;
using Fibo.Messages;
using Fibo.Processing;
using Fibo.Storage;
using Fibo.Transport;
using Moq;
using NUnit.Framework;

namespace Fibo.Tests
{
    [TestFixture]
    public class ProcessorTests
    {
        private Mock<ICalculator> _calculatorMock;
        private Mock<IStorage<string, BigInteger>> _storageMock;
        private Mock<ISender<FibonacciMessage>> _senderMock;

        [Test]
        public async Task ProcessMessageSuccess()
        {
            _calculatorMock = new Mock<ICalculator>(MockBehavior.Strict);
            _storageMock = new Mock<IStorage<string, BigInteger>>(MockBehavior.Strict);
            _senderMock = new Mock<ISender<FibonacciMessage>>(MockBehavior.Strict);

            var processor = new Processor(_senderMock.Object, _calculatorMock.Object, _storageMock.Object);

            BigInteger previousNumber = 2;
            BigInteger newNumber = 3;
            BigInteger result = 5;
            var message = new FibonacciMessage { Number = newNumber };
            const string sessionId = "1";

            var sequence = new MockSequence();
            var res = result;
            _storageMock.InSequence(sequence).Setup(m => m.GetValue(sessionId)).Returns(previousNumber);
            _calculatorMock.InSequence(sequence).Setup(m => m.Calculate(previousNumber, newNumber, out res)).Returns(true);
            _storageMock.InSequence(sequence).Setup(m => m.SetValue(sessionId, result)).Returns(true);
            _senderMock.InSequence(sequence).Setup(m => m.SendAsync(It.Is<FibonacciMessage>(x => x.Number == result), sessionId)).Returns(Task.FromResult(new Response { StatusCode = Response.OkCode }));

            var processResult = await processor.ProcessMessageAsync(message, sessionId);

            Assert.AreEqual(result, processResult.Value);
            Assert.AreEqual(false, processResult.HasError);
        }

        [Test]
        public async Task ProcessMessageCalculatorError()
        {
            _calculatorMock = new Mock<ICalculator>(MockBehavior.Strict);
            _storageMock = new Mock<IStorage<string, BigInteger>>(MockBehavior.Strict);
            _senderMock = new Mock<ISender<FibonacciMessage>>(MockBehavior.Strict);

            var processor = new Processor(_senderMock.Object, _calculatorMock.Object, _storageMock.Object);

            BigInteger previousNumber = 2;
            BigInteger newNumber = 3;
            BigInteger result = 5;
            BigInteger expected = 0;
            var message = new FibonacciMessage { Number = newNumber };
            const string sessionId = "1";

            var sequence = new MockSequence();
            var res = result;
            _storageMock.InSequence(sequence).Setup(m => m.GetValue(sessionId)).Returns(previousNumber);
            _calculatorMock.InSequence(sequence).Setup(m => m.Calculate(previousNumber, newNumber, out res)).Returns(false);
            _storageMock.InSequence(sequence).Setup(m => m.SetValue(sessionId, result)).Returns(true);
            _senderMock.InSequence(sequence).Setup(m => m.SendAsync(It.Is<FibonacciMessage>(x => x.Number == result), sessionId)).Returns(Task.FromResult(new Response { StatusCode = Response.OkCode }));

            var processResult = await processor.ProcessMessageAsync(message, sessionId);

            Assert.AreEqual(expected, processResult.Value);
            Assert.AreEqual(true, processResult.HasError);
        }

        [Test]
        public async Task ProcessMessageSenderError()
        {
            _calculatorMock = new Mock<ICalculator>(MockBehavior.Strict);
            _storageMock = new Mock<IStorage<string, BigInteger>>(MockBehavior.Strict);
            _senderMock = new Mock<ISender<FibonacciMessage>>(MockBehavior.Strict);

            var processor = new Processor(_senderMock.Object, _calculatorMock.Object, _storageMock.Object);

            BigInteger previousNumber = 2;
            BigInteger newNumber = 3;
            BigInteger result = 5;
            BigInteger expected = 0;
            var message = new FibonacciMessage { Number = newNumber };
            const string sessionId = "1";
            const string error = "Error";

            var sequence = new MockSequence();
            var res = result;
            _storageMock.InSequence(sequence).Setup(m => m.GetValue(sessionId)).Returns(previousNumber);
            _calculatorMock.InSequence(sequence).Setup(m => m.Calculate(previousNumber, newNumber, out res)).Returns(true);
            _storageMock.InSequence(sequence).Setup(m => m.SetValue(sessionId, result)).Returns(true);
            _senderMock.InSequence(sequence).Setup(m => m.SendAsync(It.Is<FibonacciMessage>(x => x.Number == result), sessionId)).Returns(Task.FromResult(new Response { StatusCode = Response.ServerErrorCode, Message = error }));

            var processResult = await processor.ProcessMessageAsync(message, sessionId);

            Assert.AreEqual(expected, processResult.Value);
            Assert.AreEqual(true, processResult.HasError);
            Assert.AreEqual(error, processResult.Error);
        }
    }
}
