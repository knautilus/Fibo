using System.Numerics;

namespace Fibo.Calculator
{
    public class FibonacciCalculator : ICalculator
    {
        public bool Calculate(BigInteger operandA, BigInteger operandB, out BigInteger result)
        {
            if (operandA == 0 && operandB != 1)
            {
                result = 0;
                return false;
            }
            result = operandA + operandB;
            return true;
        }
    }
}
