using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class CacheItem<T>
    {
        public string Cache { get; set; }

        public string Key { get; set; }

        public T Value { get; set; }

        [JsonProperty("cas")]
        public long CheckAndSet { get; set; }
    }
}