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
    }
}
