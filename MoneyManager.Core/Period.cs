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
        public static DateOnly GetEndDate(this Period period, DateOnly startDate)
            => period switch
            {
                Period.Daily => startDate,
                Period.Weekly => startDate.AddDays(6),
                Period.Fortnightly => startDate.AddDays(15),
                Period.Monthly => startDate.AddMonths(1).AddDays(-1),
                Period.Quarterly => startDate.AddMonths(3).AddDays(-1),
                Period.Biannually => startDate.AddMonths(6).AddDays(-1),
                Period.Annually => startDate.AddYears(1).AddDays(-1),
                _ => throw new ArgumentOutOfRangeException(nameof(period), "Period must not be null!")
            };

        public static int? GetNumberInOrNull(this Period smallPeriod, Period bigPeriod)
            => bigPeriod switch
            {
                Period.Daily => null, // Daily cannot be divided into any other period (except itself, which for the purpose of this app doesn't count)
                Period.Weekly => smallPeriod is Period.Daily ? 7 : null,
                Period.Fortnightly => smallPeriod switch
                {
                    Period.Daily => 14,
                    Period.Weekly => 2,
                    _ => null
                },
                Period.Monthly => null, // Months cannot be evenly divided into any other period
                Period.Quarterly => smallPeriod switch
                {
                    Period.Weekly => 13,
                    Period.Monthly => 3,
                    _ => null
                },
                Period.Biannually => smallPeriod switch
                {
                    Period.Weekly => 26,
                    Period.Fortnightly => 13,
                    Period.Monthly => 6,
                    Period.Quarterly => 2,
                    _ => null
                },
                Period.Annually => smallPeriod switch
                {
                    Period.Daily => 365,
                    Period.Weekly => 52,
                    Period.Fortnightly => 26,
                    Period.Monthly => 12,
                    Period.Quarterly => 4,
                    Period.Biannually => 2,
                    _ => null
                },
                _ => null
            };
    }
}
