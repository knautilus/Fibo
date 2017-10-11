using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Fibo.Logging;
using Fibo.Messages;
using Fibo.Processing;
using Fibo.Transport;

namespace Fibo.Second.Controllers
{
    public class FibonacciController : ApiController
    {
        private readonly IProcessor _processor;
        private readonly ILogger _logger;

        public FibonacciController(IProcessor processor,
            ILogger logger)
        {
            _processor = processor;
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
                var result = await _processor.ProcessMessageAsync(message, sessionId);
                if (result.HasError)
                {
                    _logger.Log($"{sessionId}: {result.Error}", LogEventType.Error);
                    return InternalServerError(new Exception(result.Error));
                }
                _logger.Log($"{sessionId}: {result.Value}", LogEventType.Info);
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
