using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace io.iron.ironcache.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class Item<T>
    {
        /// <summary>
        /// The item’s data
        /// </summary>
        [JsonProperty("body")]
        public T Body { get; set; }

        /// <summary>
        ///  How long in seconds to keep the item in the cache before it is deleted. Default is 604,800 seconds (7 days). Maximum is 2,592,000 seconds (30 days).
        /// </summary>
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        /// <summary>
        ///  If set to true, only set the item if the item is already in the cache. If the item is not in the cache, do not create it.
        /// </summary>
        [JsonProperty("replace")]
        public bool Replace { get; set; }

        /// <summary>
        ///  If set to true, only set the item if the item is not already in the cache. If the item is in the cache, do not overwrite it.
        /// </summary>
        [JsonProperty("add")]
        public bool Add { get; set; }
    }
}