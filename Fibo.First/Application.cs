using Fibo.Logging;
using Fibo.Messages;
using Fibo.Processing;
using Fibo.Storage;
using Fibo.Transport;
using System;
using System.Threading.Tasks;

namespace Fibo.First
{
    public class Application
    {
        private readonly ISender<FibonacciMessage> _sender;
        private readonly IConsumer<FibonacciMessage> _consumer;
        private readonly ICalculator<ulong> _calculator;
        private readonly IStorage<string, ulong> _storage;
        private readonly ILogger _logger;

        public Application(ISender<FibonacciMessage> sender,
            IConsumer<FibonacciMessage> consumer,
            ICalculator<ulong> calculator,
            IStorage<string, ulong> storage,
            ILogger logger)
        {
            _calculator = calculator;
            _sender = sender;
            _consumer = consumer;
            _storage = storage;
            _logger = logger;
        }

        public async void RunAsync(int count)
        {
            _logger.Log("Initializing transport", LogEventType.Info);
            try
            {
                _consumer.SetAction((msg, id) => ConsumeAsync(msg, id));
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, LogEventType.Error);
                return;
            }
            _logger.Log("Transport initialized", LogEventType.Info);

            for (int i = 1; i <= count; i++)
            {
                var sessionId = i.ToString();
                ulong number = 1;
                _storage.SetValue(sessionId, number);
                _logger.Log(string.Format("{0}: {1}", sessionId, number), LogEventType.Info);
                var response = await _sender.SendAsync(new FibonacciMessage { Number = number }, sessionId);
                if (response.StatusCode != Response.OkCode)
                {
                    _logger.Log(string.Format("{0}: {1}", sessionId, response.Message), LogEventType.Error);
                }
            }
        }

        private async Task ConsumeAsync(FibonacciMessage message, string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                _logger.Log("SessionId not specified", LogEventType.Error);
                return;
            }

            try
            {
                var previousNumber = _storage.GetValue(sessionId);
                if (!_calculator.Calculate(previousNumber, message.Number, out ulong result))
                {
                    _logger.Log(string.Format("{0}: {1}", sessionId, "Finished"), LogEventType.Info);
                    return;
                }
                _storage.SetValue(sessionId, result);
                _logger.Log(string.Format("{0}: {1}", sessionId, result), LogEventType.Info);
                var response = await _sender.SendAsync(new FibonacciMessage { Number = result }, sessionId);
                if (response.StatusCode != Response.OkCode)
                {
                    _logger.Log(string.Format("{0}: {1}", sessionId, response.Message), LogEventType.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(string.Format("{0}: {1}", sessionId, ex.Message), LogEventType.Error);
            }
        }
    }
}
