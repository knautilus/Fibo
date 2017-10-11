using System.Threading.Tasks;
using Fibo.Messages;

namespace Fibo.Processing
{
    public interface IProcessor
    {
        Task<ProcessResult> ProcessMessageAsync(FibonacciMessage message, string sessionId);
    }
}
