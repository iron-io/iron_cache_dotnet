using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Error
    {
        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}