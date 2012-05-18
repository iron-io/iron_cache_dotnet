using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject]
    internal class Increment
    {
        /// <summary>
        /// Value after increment
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; }

        /// <summary>
        /// Response Message
        /// </summary>
        [JsonProperty("msg")]
        public string Message { get; set; }
    }
}