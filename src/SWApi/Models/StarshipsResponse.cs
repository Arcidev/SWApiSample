using Newtonsoft.Json;
using System.Collections.Generic;

namespace SWApi.Models
{
    /// <summary>
    /// Response container for starships request
    /// </summary>
    public class StarshipsResponse
    {
        /// <summary>
        /// Url for another page of starships if any
        /// </summary>
        [JsonProperty("next")]
        public string NextPageUrl { get; set; }

        /// <summary>
        /// Total count of all starships
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Starships for current page
        /// </summary>
        [JsonProperty("results")]
        public List<Starship> Starships { get; set; }
    }
}
