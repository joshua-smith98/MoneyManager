namespace MoneyManager.Core
{
    /// <summary>
    /// Represents an amount of <see cref="Money"/> budgeted across a certain <see cref="Period"/>, and translated across all other <see cref="Period"/>s.
    /// </summary>
    public class Budget
    {
        /// <summary>
        /// Gets or sets the amount of money budgeted per day.
        /// </summary>
        public Money PerDay 
        {
            get => Get(Period.Daily);
            set => Set(value, Period.Daily);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per week.
        /// </summary>
        public Money PerWeek
        {
            get => Get(Period.Weekly);
            set => Set(value, Period.Weekly);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per fortnight.
        /// </summary>
        public Money PerFortnight
        {
            get => Get(Period.Fortnightly);
            set => Set(value, Period.Fortnightly);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per month.
        /// </summary>
        public Money PerMonth
        {
            get => Get(Period.Monthly);
            set => Set(value, Period.Monthly);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per quarter.
        /// </summary>
        public Money PerQuarter
        {
            get => Get(Period.Quarterly);
            set => Set(value, Period.Quarterly);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per 6 months.
        /// </summary>
        public Money Per6Months
        {
            get => Get(Period.Biannually);
            set => Set(value, Period.Biannually);
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per year.
        /// </summary>
        public Money PerYear
        {
            get => Get(Period.Annually);
            set => Set(value, Period.Annually);
        }

        private Money perDay;

        /// <summary>
        /// Gets the current base period for this budget.
        /// </summary>
        public Period CurrentPeriod { get; private set; }

        public Budget(Money value, Period period)
        {
            Set(value, period); // Set() sets the value of perDay, so just ignore the IDE's complaints here
        }

        /// <summary>
        /// Gets the sum of the given budgets with the given period.
        /// </summary>
        /// <param name="budget1"></param>
        /// <param name="budget2"></param>
        /// <param name="newPeriod"></param>
        /// <returns></returns>
        public static Budget Sum(Budget budget1, Budget budget2, Period newPeriod)
            => new Budget(FromDaily(budget1.perDay + budget2.perDay, newPeriod), newPeriod);

        /// <summary>
        /// Gets the amount of money budgeted for the given period.
        /// If <see cref="Period.Null"/> is given, then this method will return the default (daily) budget.
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public Money Get(Period period) => FromDaily(perDay, period);
        
        /// <summary>
        /// Sets the amount of money budgeted for the given period.
        /// If <paramref name="period"/> is set to <see cref="Period.Null"/>, then this method will use <paramref name="value"/> as if it were daily, but <see cref="CurrentPeriod"/> will be set to <see cref="Period.Null"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="period"></param>
        public void Set(Money value, Period period)
        {
            perDay = ToDaily(value, period);
            CurrentPeriod = period;
        }

        private static Money ToDaily(Money value, Period fromPeriod)
        {
            return fromPeriod switch
            {
                Period.Daily => value,
                Period.Weekly => value / 7,
                Period.Fortnightly => value / 14,
                Period.Monthly => value * 12 / 365,
                Period.Quarterly => value * 4 / 365,
                Period.Biannually => value * 2 / 365,
                Period.Annually => value / 365,
                Period.Null => value, // Support for budgets with no base period
                _ => throw new ArgumentOutOfRangeException(nameof(fromPeriod), "Invalid period.")
            };
        }

        private static Money FromDaily(Money dailyValue, Period toPeriod)
        {
            return toPeriod switch
            {
                Period.Daily => dailyValue,
                Period.Weekly => dailyValue * 7,
                Period.Fortnightly => dailyValue * 14,
                Period.Monthly => dailyValue * 365 / 12,
                Period.Quarterly => dailyValue * 365 / 4,
                Period.Biannually => dailyValue * 365 / 2,
                Period.Annually => dailyValue * 365,
                Period.Null => dailyValue, // Support for budgets with no base period
                _ => throw new ArgumentOutOfRangeException(nameof(toPeriod), "Invalid period.")
            };
        }
    }
}
