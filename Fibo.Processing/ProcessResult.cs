using System.Numerics;

namespace Fibo.Processing
{
    public class ProcessResult
    {
        public ProcessResult(BigInteger result, string errorMessage = null)
        {
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                Value = 0;
                HasError = true;
                Error = errorMessage;
            }
            else
            {
                Value = result;
                HasError = false;
                Error = string.Empty;
            }
        }

        public BigInteger Value { get; }
        public bool HasError { get; }
        public string Error { get; }
    }
}
