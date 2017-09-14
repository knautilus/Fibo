﻿namespace Fibo.Calculator
{
    public interface ICalculator<T>
    {
        bool Calculate(T operandA, T operandB, out T result);
    }
}
