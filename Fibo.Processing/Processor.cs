using System.Numerics;
using System.Threading.Tasks;
using Fibo.Calculator;
using Fibo.Messages;
using Fibo.Storage;
using Fibo.Transport;

namespace Fibo.Processing
{
    public class Processor : IProcessor
    {
        private readonly ISender<FibonacciMessage> _sender;
        private readonly ICalculator _calculator;
        private readonly IStorage<string, BigInteger> _storage;

        public Processor(ISender<FibonacciMessage> sender,
            ICalculator calculator,
            IStorage<string, BigInteger> storage)
        {
            _sender = sender;
            _calculator = calculator;
            _storage = storage;
        }

        public async Task<ProcessResult> ProcessMessageAsync(FibonacciMessage message, string sessionId)
        {
            var previousNumber = _storage.GetValue(sessionId);
            if (!_calculator.Calculate(previousNumber, message.Number, out BigInteger result))
            {
                return new ProcessResult(0, "Unable to calculate value");
            }
            _storage.SetValue(sessionId, result);
            var response = await _sender.SendAsync(new FibonacciMessage { Number = result }, sessionId);
            if (response.StatusCode != Response.OkCode)
            {
                return new ProcessResult(0, response.Message);
            }
            return new ProcessResult(result);
        }
    }
}
