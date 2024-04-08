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

    public static class PeriodExtensions
    {
        public static DateOnly GetEndDateExclusive(this Period period, DateOnly startDate)
            => period switch
            {
                Period.Daily => startDate,
                Period.Weekly => startDate.AddDays(7),
                Period.Fortnightly => startDate.AddDays(14),
                Period.Monthly => startDate.AddMonths(1),
                Period.Quarterly => startDate.AddMonths(3),
                Period.Biannually => startDate.AddMonths(6),
                Period.Annually => startDate.AddYears(1),
                _ => throw new ArgumentOutOfRangeException(nameof(period), "Period must not be null!")
            };

        public static DateOnly GetEndDateInclusive(this Period period, DateOnly startDate)
            => GetEndDateExclusive(period, startDate).AddDays(-1);

        public static int? DivideIntoOrNull(this Period bigPeriod, Period smallPeriod)
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
    }
}
