using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using io.iron.ironcache;
using io.iron.ironcache.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronCacheTests
{
    /// <summary>
    ///This is a test class for IronCacheTest and is intended
    ///to contain all IronCacheTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IronCacheTest
    {
        private const string CacheKey = "this is an arbitrary key";
        private const string CacheName = "test_cache";
        string _projectId, _token;

        public IronCacheTest()
        {
            _projectId = ConfigurationManager.AppSettings["IRONIO_PROJECT_ID"];
            _token = ConfigurationManager.AppSettings["IRONIO_TOKEN"];
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for IronCache Constructor
        ///</summary>
        [TestMethod()]
        public void IronCacheConstructorTest()
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);

            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Caches
        ///</summary>
        [TestMethod()]
        public void CachesTest()
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);
            target.Put(CacheName, CacheKey, "dummy value");
            IList<Cache> actual = target.Caches();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count >= 1, "At least one cache should be present after inserting a value.");
        }

        [TestMethod()]
        public void TestPutAndGetObject()
        {
            var value = new
            {
                Name = "Boombox",
                Description = "Loud",
                IsCool = true
            };

            var actual = TestPutAndGet(value);

            Assert.IsNotNull(actual);
            Assert.AreEqual(value.Name, actual.Name);
            Assert.AreEqual(value.Description, actual.Description);
            Assert.AreEqual(value.IsCool, actual.IsCool);
        }

        [TestMethod()]
        public void TestPutAndGetString()
        {
            var value = "This is a test";

            var actual = TestPutAndGet(value);

            Assert.IsNotNull(actual);
            Assert.AreEqual(value, actual);
        }

        [TestMethod()]
        public void TestPutAndGetDateTime()
        {
            var value = DateTime.Parse("2012-07-04");

            var actual = TestPutAndGet(value);

            Assert.IsNotNull(actual);
            Assert.AreEqual(value, actual);
        }

        [TestMethod()]
        public void TestPutAndGetInt()
        {
            var value = 10;

            var actual = TestPutAndGet(value);

            Assert.IsNotNull(actual);
            Assert.AreEqual(value, actual);
        }

        [TestMethod()]
        public void GetMissingValueTest()
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);

            string key = "this is an arbitrary key";
            string cache = CacheName;
            RemoveFromCache(target, cache, key);

            var actual = target.Get<string>(cache, key);
            Assert.IsNull(actual);
        }

        [TestMethod()]
        public void IncrementNonExistingTest()
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);

            string key = "82de17a0-cab9-45a5-a851-bccb210a9e1f";
            string cache = CacheName;
            RemoveFromCache(target, cache, key);
            
            var actual = target.Increment(cache, key, 1);
            
            Assert.AreEqual(actual, 1, "Increment operation was not performed since value was not present, but value was added to the cache.");
        }

        [TestMethod()]
        public void IncrementExistingIntegerTest()
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);

            string key = "cf435dc2-7f12-4f37-94c2-26077b3cd414"; // random unique identifier
            string cache = CacheName;
            target.Put(cache, key, 0, false, false, 10);
            var expected = 1;
            var actual = target.Increment(cache, key, 1);
            Assert.AreEqual(expected, actual);
        }

        private static void RemoveFromCache(IronCache target, string cache, string key)
        {
            try
            {
                target.Remove(cache, key);
            }
            catch(KeyNotFoundException)
            {
                // ignore KeyNotFound exception - we just want to be sure that the value is gone.
            }
        }


        private T TestPutAndGet<T>(T value)
        {
            string projectId = _projectId;
            string token = _token;
            IronCache target = new IronCache(projectId, token);

            string key = "this is an arbitrary key";
            string cache = CacheName;

            target.Put(cache, key, value);

            var actual = target.Get<T>(cache, key);
            return actual;
        }
    }
}