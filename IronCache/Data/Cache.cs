using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Cache
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}