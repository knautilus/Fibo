using System.Numerics;

namespace Fibo.Calculator
{
    public interface ICalculator
    {
        bool Calculate(BigInteger operandA, BigInteger operandB, out BigInteger result);
    }
}
