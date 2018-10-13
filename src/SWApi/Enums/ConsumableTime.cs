
namespace SWApi.Enums
{
    /// <summary>
    /// Represents time in hours for defined period
    /// </summary>
    public enum ConsumableTime
    {
        /// <summary>
        /// Represents 1 hour
        /// </summary>
        Hour = 1,
        /// <summary>
        /// Represents 1 hour
        /// </summary>
        Hours = Hour,

        /// <summary>
        /// Represents hours in 1 day
        /// </summary>
        Day = 24,
        /// <summary>
        /// Represents hours in 1 day
        /// </summary>
        Days = Day,

        /// <summary>
        /// Represents hours in 1 week
        /// </summary>
        Week = 7 * Day,
        /// <summary>
        /// Represents hours in 1 week
        /// </summary>
        Weeks = Week,

        /// <summary>
        /// Represents hours in 1 month
        /// </summary>
        Month = 30 * Day,
        /// <summary>
        /// Represents hours in 1 month
        /// </summary>
        Months = Month,

        /// <summary>
        /// Represents hours in 1 year
        /// </summary>
        Year = 365 * Day,
        /// <summary>
        /// Represents hours in 1 year
        /// </summary>
        Years = Year
    }
}
