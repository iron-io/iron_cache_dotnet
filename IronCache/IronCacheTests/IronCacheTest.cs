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

        #region Additional test attributes

        #endregion Additional test attributes

        /// <summary>
        ///A test for IronCache Constructor
        ///</summary>
        [TestMethod()]
        public void IronCacheConstructorTest()
        {
            Client client = null;
            IronCache target = new IronCache(client);
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
            Client client = new Client(projectId, token, "cache-aws-us-east-1.iron.io");
            IronCache target = new IronCache(client);
            var expected = 1;
            IList<Cache> actual;
            actual = target.Caches();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.Count);
        }

        [TestMethod()]
        public void AddGetTest()
        {
            string projectId = _projectId;
            string token = _token;
            Client client = new Client(projectId, token, "cache-aws-us-east-1.iron.io");
            IronCache target = new IronCache(client);

            string value = "this is some arbitrary text";
            string key = "this is an arbitrary key";
            string cache = "test_cache";
            var item = new Item()
            {
                Body = value,
                ExpiresIn = 10
            };
            target.Add(cache, key, item);
            var actual = target.Get(cache, key);
            Assert.IsNotNull(actual);
            Assert.AreEqual(value, actual.Value);
        }

        [TestMethod()]
        public void IncrementTest()
        {
            string projectId = _projectId;
            string token = _token;
            Client client = new Client(projectId, token, "cache-aws-us-east-1.iron.io");
            IronCache target = new IronCache(client);

            string key = "82de17a0-cab9-45a5-a851-bccb210a9e1f";
            string cache = "test_cache";
            target.Remove(cache, key);
            var expected = 1;
            var actual = target.Increment(cache, key, 1);
            Assert.AreEqual(expected, actual);
        }
    }
}