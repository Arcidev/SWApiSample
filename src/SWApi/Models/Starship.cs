using Newtonsoft.Json;
using SWApi.Enums;
using System;

namespace SWApi.Models
{
    /// <summary>
    /// Starship model provided from Star Wars API
    /// </summary>
    public class Starship
    {
        /// <summary>
        /// The name of this starship
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The Maximum number of Megalights this starship can travel in a standard hour
        /// </summary>
        public string MGLT { get; set; }

        /// <summary>
        /// The maximum length of time that this starship can provide consumables for its entire crew without having to resupply
        /// </summary>
        [JsonProperty("consumables")]
        public string Consumables { get; set; }

        /// <summary>
        /// Calculates required stops for this starship based on distance
        /// </summary>
        /// <param name="distance">Distance to be made</param>
        /// <returns>Stops required to make the flight if <see cref="MGLT"/> and <see cref="Consumables"/> are known, otherwise null</returns>
        public int? CalculateStops(int distance)
        {
            if (!int.TryParse(MGLT, out var mglt) || mglt == 0)
                return null;

            var consumables = GetHoursFromConsumables();
            if (!consumables.HasValue || consumables == 0)
                return null;

            var hoursToRefill = mglt * consumables;
            var result = distance / hoursToRefill;

            // Subtract 1 stop if distance is multiplication of hoursToRefill as we've arrived into destination when stop is required
            return distance % hoursToRefill == 0 ? --result : result;
        }

        private int? GetHoursFromConsumables()
        {
            var consumables = Consumables?.Split();
            if (consumables?.Length != 2 || !int.TryParse(consumables[0], out var consumableValue))
                return null;

            if (!Enum.TryParse(consumables[1], true, out ConsumableTime consumableTime))
                return null;

            return consumableValue * (int)consumableTime;
        }
    }
}
