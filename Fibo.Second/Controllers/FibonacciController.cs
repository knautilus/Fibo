﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Fibo.Logging;
using Fibo.Messages;
using Fibo.Calculator;
using Fibo.Storage;
using Fibo.Transport;

namespace Fibo.Second.Controllers
{
    public class FibonacciController : ApiController
    {
        private readonly ISender<FibonacciMessage> _sender;
        private readonly ICalculator<ulong> _calculator;
        private readonly IStorage<string, ulong> _storage;
        private readonly ILogger _logger;

        public FibonacciController(ISender<FibonacciMessage> sender,
            ICalculator<ulong> calculator,
            IStorage<string, ulong> storage,
            ILogger logger)
        {
            _sender = sender;
            _calculator = calculator;
            _storage = storage;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] FibonacciMessage message)
        {
            var sessionId = Request.Headers.GetValues(Constants.SessionIdHeader).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                const string error = "SessionId not specified";
                _logger.Log(error, LogEventType.Error);
                return BadRequest(error);
            }

            try
            {
                var previousNumber = _storage.GetValue(sessionId);
                if (!_calculator.Calculate(previousNumber, message.Number, out ulong result))
                {
                    _logger.Log($"{sessionId}: Finished", LogEventType.Info);
                    return Ok();
                }
                _storage.SetValue(sessionId, result);
                _logger.Log($"{sessionId}: {result}", LogEventType.Info);
                var response = await _sender.SendAsync(new FibonacciMessage { Number = result }, sessionId);
                if (response.StatusCode != Response.OkCode)
                {
                    _logger.Log($"{sessionId}: {response.Message}", LogEventType.Error);
                    return InternalServerError(response.Exception);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log($"{sessionId}: {ex.Message}", LogEventType.Error);
                return InternalServerError(ex);
            }
        }
    }
}
