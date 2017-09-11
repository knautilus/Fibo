using System;

namespace Fibo.Processing
{
    public class FibonacciCalculator : ICalculator<ulong>
    {
        public bool Calculate(ulong operandA, ulong operandB, out ulong result)
        {
            if (operandA == 0 && operandB != 1)
            {
                throw new ArgumentException("Invalid operands");
            }
            try
            {
                result = checked(operandA + operandB);
                return true;
            }
            catch (OverflowException)
            {
                result = 0;
                return false;
            }
        }
    }
}
