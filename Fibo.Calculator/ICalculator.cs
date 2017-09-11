namespace Fibo.Processing
{
    public interface ICalculator<T>
    {
        ulong Calculate(T operandA, T operandB);
    }
}
