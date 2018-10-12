using Newtonsoft.Json;
using SWApi.Extensions;

namespace SWApi.Models
{
    public class Starship
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        public string MGLT { get; set; }

        [JsonProperty("consumables")]
        public string Consumables { get; set; }

        public int? CalculateStops(int distance)
        {
            if (!int.TryParse(MGLT, out var mglt) || mglt == 0)
                return null;

            var consumables = this.GetHoursFromConsumables();
            if (!consumables.HasValue || consumables == 0)
                return null;

            return distance / (mglt * consumables);
        }
    }
}
