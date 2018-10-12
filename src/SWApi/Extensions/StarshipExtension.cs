using SWApi.Models;

namespace SWApi.Extensions
{
    public static class StarshipExtension
    {
        private const int hoursInDay = 24;
        private const int hoursInMonth = 30 * hoursInDay;
        private const int hoursInYear = 365 * hoursInDay;

        public static int? GetHoursFromConsumables(this Starship starship)
        {
            var consumables = starship.Consumables?.Split();
            if (consumables?.Length != 2 || !int.TryParse(consumables[0], out var consumableValue))
                return null;

            switch (consumables[1])
            {
                case "day":
                case "days":
                    return consumableValue * hoursInDay;
                case "month":
                case "months":
                    return consumableValue * hoursInMonth;
                case "year":
                case "years":
                    return consumableValue * hoursInYear;
                default:
                    return null;
            }
        }
    }
}
