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

        public IronCache(Client client)
        {
            _client = client;
        }

        public IList<Cache> Caches()
        {
            string method = "caches";
            var response = _client.get(method);
            var caches = JsonConvert.DeserializeObject<List<Cache>>(response) ?? new List<Cache>();
            return caches as IList<Cache>;
        }

        public void Add(string cache, string key, Item item)
        {
            string endpoint = string.Format("caches/{0}/items/{1}", cache, key);
            var body = JsonConvert.SerializeObject(item);
            var response = _client.put(endpoint, body);
        }

        public CacheItem Get(string cache, string key)
        {
            string endpoint = string.Format("caches/{0}/items/{1}", cache, key);
            var response = _client.get(endpoint);
            var item = JsonConvert.DeserializeObject<CacheItem>(response);
            return item;
        }

        public void Remove(string cache, string key)
        {
            string endpoint = string.Format("caches/{0}/items/{1}", cache, key);
            var response = _client.delete(endpoint);
        }

        public int Increment(string cache, string key, int amount)
        {
            string endpoint = string.Format("caches/{0}/items/{1}/increment", cache, key);
            var body = "{" + string.Format("amount: {0}", amount) + "}";
            var response = _client.post(endpoint, body);
            var increment = JsonConvert.DeserializeObject<Increment>(response);
            return increment.Value;
        }
    }
}