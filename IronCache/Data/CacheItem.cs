using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CacheItem
    {
        public string Cache { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}