using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.iron.ironcache.Data;
using Newtonsoft.Json;

namespace io.iron.ironcache
{
    public class IronCache
    {
        private Client _client;
        private static string _core = "caches";

        public IronCache(string projectId, string token)
        {
            _client = new Client(projectId, token, "cache-aws-us-east-1.iron.io");
        }

        public IList<Cache> Caches()
        {
            string method = _core;
            var response = _client.get(method);
            var caches = JsonConvert.DeserializeObject<List<Cache>>(response) ?? new List<Cache>();
            return caches as IList<Cache>;
        }

        /// <summary>
        ///  Only set the item if the item is not already in the cache. If the item is in the cache, do not overwrite it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">Name of the cache</param>
        /// <param name="key">The key to store the item under in the cache</param>
        /// <param name="value">Item to be stored</param>
        /// <param name="expiresIn"> How long in seconds to keep the item in the cache before it is deleted. Default is 604,800 seconds (7 days). Maximum is 2,592,000 seconds (30 days).</param>
        public void Add<T>(string cache, string key, T value, int expiresIn = 0)
        {
            Put<T>(cache, key, value, true, false, expiresIn);
        }

        /// <summary>
        /// Sets the value at the key in the cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">Name of the cache</param>
        /// <param name="key">The key to store the item under in the cache</param>
        /// <param name="value">Item to be stored</param>
        /// <param name="expiresIn"> How long in seconds to keep the item in the cache before it is deleted. Default is 604,800 seconds (7 days). Maximum is 2,592,000 seconds (30 days).</param>
        public void Set<T>(string cache, string key, T value, int expiresIn = 0)
        {
            Put<T>(cache, key, value, false, false, expiresIn);
        }

        /// <summary>
        /// Only set the item if the item is already in the cache. If the item is not in the cache, do not create it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key">The key to store the item under in the cache</param>
        /// <param name="value">Item to be stored</param>
        /// <param name="expiresIn"> How long in seconds to keep the item in the cache before it is deleted. Default is 604,800 seconds (7 days). Maximum is 2,592,000 seconds (30 days).</param>
        public void Replace<T>(string cache, string key, T value, int expiresIn = 0)
        {
            Put<T>(cache, key, value, false, true, expiresIn);
        }

        /// <summary>
        /// Puts a value into the cache for the provided key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache">Name of the cache</param>
        /// <param name="key">The key to store the item under in the cache</param>
        /// <param name="value">Item to be stored</param>
        /// <param name="add">If set to true, only set the item if the item is not already in the cache. If the item is in the cache, do not overwrite it.</param>
        /// <param name="replace">If set to true, only set the item if the item is already in the cache. If the item is not in the cache, do not create it.</param>
        /// <param name="expiresIn"> How long in seconds to keep the item in the cache before it is deleted. Default is 604,800 seconds (7 days). Maximum is 2,592,000 seconds (30 days).</param>
        public void Put<T>(string cache, string key, T value, bool add = false, bool replace = false, int expiresIn = 0)
        {
            if (add && replace) throw new ArgumentException("add and replace cannot both be true.");
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);
            var item = new Item<T>() { Body = value, Add = add, Replace = replace, ExpiresIn = expiresIn };
            var body = JsonConvert.SerializeObject(item);
            var response = _client.put(endpoint, body);
        }

        /// <summary>
        /// Internal Get method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"> The name of the cache the item belongs to.</param>
        /// <param name="key">The key the item is stored under in the cache.</param>
        /// <param name="defaultValue">Value to return if not found</param>
        /// <returns>Value at the key or the default value if not found</returns>
        public T Get<T>(string cache, string key, T defaultValue = default(T))
        {
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);
            try
            {
                var response = _client.get(endpoint);
                var item = JsonConvert.DeserializeObject<CacheItem<T>>(response);
                return item.Value;
            }
            catch (System.Web.HttpException we)
            {
                if (we.GetHttpCode() == 404)
                {
                    return defaultValue;
                }
                throw we;
            }
        }

        public void Remove(string cache, string key)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);

            var response = _client.delete(endpoint);
        }

        /// <summary>
        /// Increment an existing integer
        /// </summary>
        /// <param name="cache">Cache Name</param>
        /// <param name="key">Key to lookkup</param>
        /// <param name="amount">amount to increment (decrement) value at the specified key</param>
        /// <returns>value after increment</returns>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException"></exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public int Increment(string cache, string key, int amount)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}/increment", _core, cache, key);

            var body = "{" + string.Format("\"amount\": {0}", amount) + "}";
            try
            {
                var response = _client.post(endpoint, body);

                var increment = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                return int.Parse(increment["value"]);
            }
            catch (System.Web.HttpException e)
            {
                if (e.GetHttpCode() == 404)
                {
                    throw new KeyNotFoundException(e.Message);
                }
                if (e.GetHttpCode() == 400)
                {
                    throw new InvalidOperationException(e.Message);
                }
                throw e;
            }
        }
    }
}