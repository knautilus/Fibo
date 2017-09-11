using System;

namespace Fibo.Processing
{
    public class FibonacciCalculator : ICalculator<ulong>
    {
        public ulong Calculate(ulong operandA, ulong operandB)
        {
            if (operandA == 0 && operandB != 1)
            {
                throw new ArgumentException("Invalid operands");
            }
            return checked(operandA + operandB);
        }
    }
}
