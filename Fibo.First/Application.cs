using Fibo.Logging;
using Fibo.Messages;
using Fibo.Processing;
using Fibo.Transport;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Fibo.First
{
    public class Application : IApplication
    {
        private readonly IConsumer<FibonacciMessage> _consumer;
        private readonly IProcessor _processor;
        private readonly ILogger _logger;

        public Application(IConsumer<FibonacciMessage> consumer,
            IProcessor processor,
            ILogger logger)
        {
            _processor = processor;
            _consumer = consumer;
            _logger = logger;
        }

        public async void RunAsync(int count)
        {
            _logger.Log("Initializing transport", LogEventType.Info);
            try
            {
                _consumer.SetAction(ConsumeAsync);
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
                BigInteger number = 1;

                await ConsumeAsync(new FibonacciMessage {Number = number}, sessionId);
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
                var result = await _processor.ProcessMessageAsync(message, sessionId);
                if (result.HasError)
                {
                    _logger.Log($"{sessionId}: {result.Error}", LogEventType.Error);
                }
                _logger.Log($"{sessionId}: {result.Value}", LogEventType.Info);
            }
            catch (Exception ex)
            {
                _logger.Log($"{sessionId}: {ex.Message}", LogEventType.Error);
            }
        }
    }
}
