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

        public void Add(string cache, string key, string value, bool add = false, bool replace = false, int expiresIn = 0)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);
            var item = new Item() { Body = value, Add = add, Replace = replace, ExpiresIn = expiresIn };
            var body = JsonConvert.SerializeObject(item);
            var response = _client.put(endpoint, body);
        }

        public string Get(string cache, string key)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);

            var response = _client.get(endpoint);
            var item = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return item["value"];
        }

        public void Remove(string cache, string key)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}", _core, cache, key);

            var response = _client.delete(endpoint);
        }

        public int Increment(string cache, string key, int amount)
        {
            string endpoint = string.Format("{0}/{1}/items/{2}/increment", _core, cache, key);

            var body = "{" + string.Format("\"amount\": {0}", amount) + "}";
            var response = _client.post(endpoint, body);

            var increment = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
            return int.Parse(increment["value"]);
        }
    }
}