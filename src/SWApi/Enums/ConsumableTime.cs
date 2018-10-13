
namespace SWApi.Enums
{
    /// <summary>
    /// Represents time in hours for defined period
    /// </summary>
    public enum ConsumableTime
    {
        Day = 24,
        Days = Day,

        Week = 7 * Day,
        Weeks = Week,

        Month = 30 * Day,
        Months = Month,

        Year = 365 * Day,
        Years = Year
    }
}
