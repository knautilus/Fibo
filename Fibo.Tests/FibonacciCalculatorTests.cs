using Fibo.Calculator;
using NUnit.Framework;
using System.Numerics;

namespace Fibo.Tests
{
    [TestFixture]
    public class FibonacciCalculatorTests
    {
        private ICalculator _calculator;

        [SetUp]
        public void SetUp()
        {
            _calculator = new FibonacciCalculator();
        }

        [Test]
        public void CorrectSequenceBeginning()
        {
            BigInteger expected = 1;
            var calculated = _calculator.Calculate(0, 1, out BigInteger result);
            Assert.AreEqual(true, calculated);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WrongSequenceBeginning()
        {
            var calculated = _calculator.Calculate(0, 8, out BigInteger result);
            Assert.AreEqual(false, calculated);
        }

        [Test]
        public void BigNumbers()
        {
            BigInteger expected = 12200160415121876738;
            var calculated = _calculator.Calculate(4660046610375530309, 7540113804746346429, out BigInteger result);
            Assert.AreEqual(true, calculated);
            Assert.AreEqual(expected, result);
        }
    }
}
