using Newtonsoft.Json;
using System.Collections.Generic;

namespace SWApi.Models
{
    public class StarshipsResponse
    {
        [JsonProperty("next")]
        public string NextPageUrl { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("results")]
        public List<Starship> Starships { get; set; }
    }
}
