namespace MoneyManager.Core
{
    /// <summary>
    /// Represents a cyclic period of time. Used in <see cref="Budget"/>s and reports.
    /// </summary>
    public enum Period
    {
        Null,
        Daily,
        Weekly,
        Fortnightly,
        Monthly,
        Quarterly,
        Biannually,
        Annually
    }

    /// <summary>
    /// Extensions for the <see cref="Period"/> enum.
    /// </summary>
    public static class PeriodExtensions
    {
        /// <summary>
        /// Returns <see cref="true"/> if this <see cref="Period"/> is longer than the given one.
        /// </summary>
        /// <param name="period1"></param>
        /// <param name="period2"></param>
        /// <returns></returns>
        public static bool IsLongerThan(this Period period1, Period period2)
            => period1 is Period.Null || period2 is Period.Null ? false : period1 > period2; // Taking advantage of the order of values in the enum

        /// <summary>
        /// Returns <see cref="true"/> if this <see cref="Period"/> is shorter than the given one.
        /// </summary>
        /// <param name="period1"></param>
        /// <param name="period2"></param>
        /// <returns></returns>
        public static bool IsShorterThan(this Period period1, Period period2)
            => !(period1.IsLongerThan(period2) || period1 == period2); // Not longer than or equal to
        
        /// <summary>
        /// Gets the date this <see cref="Period"/> would end, if starting from the given date. Exclusive.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DateOnly GetEndDateExclusive(this Period period, DateOnly startDate)
            => period switch
            {
                Period.Daily => startDate.AddDays(1),
                Period.Weekly => startDate.AddDays(7),
                Period.Fortnightly => startDate.AddDays(14),
                Period.Monthly => startDate.AddMonths(1),
                Period.Quarterly => startDate.AddMonths(3),
                Period.Biannually => startDate.AddMonths(6),
                Period.Annually => startDate.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(period), "Period must not be null!")
            };

        /// <summary>
        /// Gets the date this <see cref="Period"/> would end, if starting from the given date. Inclusive.
        /// </summary>
        /// <param name="period"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public static DateOnly GetEndDateInclusive(this Period period, DateOnly startDate)
            => GetEndDateExclusive(period, startDate).AddDays(-1);

        /// <summary>
        /// Attempts to divide this <see cref="Period"/> evenly by a smaller period.
        /// Returns <see cref="null"/> if the division can't be done evenly, the given period is larger than this one, or they are equal.
        /// </summary>
        /// <param name="bigPeriod"></param>
        /// <param name="smallPeriod"></param>
        /// <returns></returns>
        [Obsolete] // I'd remove it, but I just love it so much...
        public static int? DivideIntoEvenlyOrNull(this Period bigPeriod, Period smallPeriod)
        {
            if (bigPeriod == smallPeriod) return null; // For the sake of this app, a Period can't divide into itself

            // Get big end date
            DateOnly startDate = DateOnly.MinValue;
            DateOnly endDate = bigPeriod.GetEndDateExclusive(startDate);

            // Iterate through small end dates until we reach the big end, or overtake it.
            DateOnly stepDate = startDate;
            int counter = 0;

            while (stepDate < endDate)
            {
                stepDate = smallPeriod.GetEndDateExclusive(stepDate);
                counter++;
            }

            if (stepDate == endDate) return counter; // The big period divides into the small period evenly
            return null; // if stepDate > endDate: The big period does not divide evenly into the small period
        }

        /// <summary>
        /// Divides this <see cref="Period"/> by another one, rounding up. Returns 0 if the given period is larger than this one.
        /// </summary>
        /// <param name="bigPeriod"></param>
        /// <param name="smallPeriod"></param>
        /// <returns></returns>
        public static int DivideInto(this Period bigPeriod, Period smallPeriod)
        {
            // Special case: small period is bigger than big period -> result is 0
            if (smallPeriod.IsLongerThan(bigPeriod)) return 0;
            
            // Get big end date
            DateOnly startDate = DateOnly.MinValue;
            DateOnly endDate = bigPeriod.GetEndDateExclusive(startDate);

            // Iterate through small end dates until we reach the big end, or overtake it.
            DateOnly stepDate = startDate;
            int counter = 0;

            while (stepDate < endDate)
            {
                stepDate = smallPeriod.GetEndDateExclusive(stepDate);
                counter++;
            }

            return counter;
        }
    }
}
