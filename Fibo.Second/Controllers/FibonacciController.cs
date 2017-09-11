using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Fibo.Logging;
using Fibo.Messages;
using Fibo.Processing;
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
                var error = "SessionId not specified";
                _logger.Log(error, LogEventType.Error);
                return BadRequest(error);
            }

            ulong result = 0;

            try
            {
                var previousNumber = _storage.GetValue(sessionId);
                result = _calculator.Calculate(previousNumber, message.Number);
                _storage.SetValue(sessionId, result);
                _logger.Log(string.Format("{0}: {1}", sessionId, result), LogEventType.Info);
            }
            catch (OverflowException)
            {
                _logger.Log(string.Format("{0}: {1}", sessionId, "Finished"), LogEventType.Info);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(string.Format("{0}: {1}", sessionId, ex.Message), LogEventType.Error);
                return InternalServerError(ex);
            }

            var response = await _sender.SendAsync(new FibonacciMessage { Number = result }, sessionId);
            if (response.StatusCode != Response.OkCode)
            {
                _logger.Log(string.Format("{0}: {1}", sessionId, response.Message), LogEventType.Error);
                return InternalServerError(response.Exception);
            }
            return Ok();
        }
    }
}
