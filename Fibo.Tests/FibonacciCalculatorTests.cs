using Fibo.Calculator;
using NUnit.Framework;
using System;

namespace Fibo.Tests
{
    [TestFixture]
    public class FibonacciCalculatorTests
    {
        private ICalculator<ulong> _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new FibonacciCalculator();
        }

        [Test]
        public void CorrectSequenceBeginning()
        {
            var calculated = _calculator.Calculate(0, 1, out ulong result);
            Assert.AreEqual(true, calculated);
            Assert.AreEqual(1, result);
        }

        [Test]
        public void WrongSequenceBeginning()
        {
            Assert.Throws(typeof(ArgumentException), () => _calculator.Calculate(0, 8, out ulong result));
        }

        [Test]
        public void BigNumbers()
        {
            var calculated = _calculator.Calculate(4660046610375530309, 7540113804746346429, out ulong result);
            Assert.AreEqual(true, calculated);
            Assert.AreEqual(12200160415121876738, result);
        }

        [Test]
        public void ULongOverflow()
        {
            var calculated = _calculator.Calculate(7540113804746346429, 12200160415121876738, out ulong result);
            Assert.AreEqual(false, calculated);
            Assert.AreEqual(0, result);
        }
    }
}
