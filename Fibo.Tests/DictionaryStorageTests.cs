using Fibo.Storage;
using NUnit.Framework;

namespace Fibo.Tests
{
    [TestFixture]
    public class DictionaryStorageTests
    {
        private IStorage<string, ulong> _storage;

        [SetUp]
        public void SetUp()
        {
            _storage = new DictionaryStorage<string, ulong>();
        }

        [Test]
        public void SetValue()
        {
            Assert.AreEqual(true, _storage.SetValue("key", 1));
        }

        [Test]
        public void GetNonexistentValue()
        {
            var result = _storage.GetValue("key");
            Assert.AreEqual(default(ulong), result);
        }

        [Test]
        public void GetAfterSet()
        {
            ulong value1 = 3;
            ulong value2 = 5;
            _storage.SetValue("key1", value1);
            _storage.SetValue("key2", value2);
            var result1 = _storage.GetValue("key1");
            Assert.AreEqual(value1, result1);
            var result2 = _storage.GetValue("key2");
            Assert.AreEqual(value2, result2);
        }

        [Test]
        public void GetAfterUpdate()
        {
            ulong value1 = 3;
            ulong value2 = 5;
            _storage.SetValue("key1", value1);
            _storage.SetValue("key1", value2);
            var result = _storage.GetValue("key1");
            Assert.AreEqual(value2, result);
        }
    }
}
