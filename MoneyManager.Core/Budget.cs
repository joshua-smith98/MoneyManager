namespace MoneyManager.Core
{
    /// <summary>
    /// Represents an amount of <see cref="Money"/> budgeted across a certain <see cref="Period"/>, and translated across all other <see cref="Period"/>s.
    /// </summary>
    public class Budget
    {
        private Money? perDay;
        private Money? perWeek;
        private Money? perFortnight;
        private Money? perMonth;
        private Money? perQuarter;
        private Money? per6Months;
        private Money? perYear;

        /// <summary>
        /// Gets or sets the amount of money per day.
        /// </summary>
        public Money PerDay
        {
            get
            {
                // Try to get from this period
                if (perDay is not null) return perDay;

                // Convert from other periods
                // Here comes the madness...
                if (perWeek is not null) return perWeek / 7;
                if (perFortnight is not null) return perFortnight / 14;
                if (perMonth is not null) return (perMonth * 12) / 365;
                if (perQuarter is not null) return (perQuarter * 4) / 365;
                if (per6Months is not null) return (per6Months * 2) / 365;
                if (perYear is not null) return perYear / 365;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = value; // <-----

                perWeek = null;
                perFortnight = null;
                perMonth = null;
                perQuarter = null;
                per6Months = null;
                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Daily;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money per week.
        /// </summary>
        public Money PerWeek
        {
            get
            {
                // Try to get from this period
                if (perWeek is not null) return perWeek;

                // Convert from other periods
                // Here comes the madness...
                if (perDay is not null) return perDay * 7;
                if (perFortnight is not null) return perFortnight / 2;
                if (perMonth is not null) return (perMonth * 12) / 52;
                if (perQuarter is not null) return (perQuarter * 4) / 52;
                if (per6Months is not null) return (per6Months * 2) / 52;
                if (perYear is not null) return perYear / 52;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;

                perWeek = value; // <-----

                perFortnight = null;
                perMonth = null;
                perQuarter = null;
                per6Months = null;
                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Weekly;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money per fortnight.
        /// </summary>
        public Money PerFortnight
        {
            get
            {
                // Try to get from this period
                if (perFortnight is not null) return perFortnight;

                // Convert from other types
                // Here comes the madness...
                if (perDay is not null) return perDay * 14;
                if (perWeek is not null) return perWeek * 2;
                if (perMonth is not null) return (perMonth * 12) / 26;
                if (perQuarter is not null) return (perQuarter * 4) / 26;
                if (per6Months is not null) return (per6Months * 2) / 26;
                if (perYear is not null) return perYear / 26;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;
                perWeek = null;

                perFortnight = value; // <-----

                perMonth = null;
                perQuarter = null;
                per6Months = null;
                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Fortnightly;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money per month.
        /// </summary>
        public Money PerMonth
        {
            get
            {
                // Try to get from this period
                if (perMonth is not null) return perMonth;

                // Convert from other types
                // Here comes the madness...
                if (perDay is not null) return (perDay * 365) / 12;
                if (perWeek is not null) return (perWeek * 52) / 12;
                if (perFortnight is not null) return (perFortnight * 26) / 12;
                if (perQuarter is not null) return perQuarter / 3;
                if (per6Months is not null) return per6Months / 6;
                if (perYear is not null) return perYear / 12;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;
                perWeek = null;
                perFortnight = null;

                perMonth = value; // <-----

                perQuarter = null;
                per6Months = null;
                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Monthly;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money per quarter.
        /// </summary>
        public Money PerQuarter
        {
            get
            {
                // Try to get from this period
                if (perQuarter is not null) return perQuarter;

                // Convert from other types
                // Here comes the madness...
                if (perDay is not null) return (perDay * 365) / 4;
                if (perWeek is not null) return (perWeek * 52) / 4;
                if (perFortnight is not null) return (perFortnight * 26) / 4;
                if (perMonth is not null) return perMonth * 3;
                if (per6Months is not null) return per6Months / 2;
                if (perYear is not null) return perYear / 4;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;
                perWeek = null;
                perFortnight = null;
                perMonth = null;

                perQuarter = value; // <-----

                per6Months = null;
                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Quarterly;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money every 6 months.
        /// </summary>
        public Money Per6Months
        {
            get
            {
                // Try to get from this period
                if (per6Months is not null) return per6Months;

                // Convert from other types
                // Here comes the madness...
                if (perDay is not null) return (perDay * 365) / 2;
                if (perWeek is not null) return (perWeek * 52) / 2;
                if (perFortnight is not null) return (perFortnight * 26) / 2;
                if (perMonth is not null) return perMonth * 6;
                if (perQuarter is not null) return perQuarter * 2;
                if (perYear is not null) return perYear / 2;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;
                perWeek = null;
                perFortnight = null;
                perMonth = null;
                perQuarter = null;

                per6Months = value; // <-----

                perYear = null;

                // Set the base period
                CurrentPeriod = Period.Biannually;
            }
        }

        /// <summary>
        /// Gets or sets the amount of money budgeted per year.
        /// </summary>
        public Money PerYear
        {
            get
            {
                // Try to get from this period
                if (perYear is not null) return perYear;

                // Convert from other types
                // Here comes the madness...
                if (perDay is not null) return perDay * 365;
                if (perWeek is not null) return perWeek * 52;
                if (perFortnight is not null) return perFortnight * 26;
                if (perMonth is not null) return perMonth * 6;
                if (perQuarter is not null) return perQuarter * 4;
                if (per6Months is not null) return per6Months * 2;

                throw new BudgetException("All Budget values are empty!");
            }
            set
            {
                // Set this period to value, and all others to null
                perDay = null;
                perWeek = null;
                perFortnight = null;
                perMonth = null;
                perQuarter = null;
                per6Months = null;

                perYear = value; // <-----

                // Set the base period
                CurrentPeriod = Period.Annually;
            }
        }

        /// <summary>
        /// The base period this budget is currently using.
        /// </summary>
        public Period CurrentPeriod { get; private set; }

        public Budget(Money value, Period period)
        {
            // Set period field to value based on period given
            switch (period)
            {
                case Period.Daily:
                    perDay = value;
                    break;
                case Period.Weekly:
                    perWeek = value;
                    break;
                case Period.Fortnightly:
                    perFortnight = value;
                    break;
                case Period.Monthly:
                    perMonth = value;
                    break;
                case Period.Quarterly:
                    perQuarter = value;
                    break;
                case Period.Biannually:
                    per6Months = value;
                    break;
                case Period.Annually:
                    perYear = value;
                    break;
                default:
                    throw new BudgetException("Budget must be defined with a valid period!");
            }

            // Set the base period
            CurrentPeriod = period;
        }

        /// <summary>
        /// Gets a <see cref="Money"/> value from this <see cref="Budget"/> for the given <see cref="Period"/>.
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="BudgetException"></exception>
        public Money Get(Period period)
        {
            switch (period)
            {
                case Period.Daily:
                    return PerDay;
                case Period.Weekly:
                    return PerWeek;
                case Period.Fortnightly:
                    return PerFortnight;
                case Period.Monthly:
                    return PerMonth;
                case Period.Quarterly:
                    return PerQuarter;
                case Period.Biannually:
                    return Per6Months;
                case Period.Annually:
                    return PerYear;
                default:
                    throw new BudgetException("Budget must be accessed with a valid period!");
            }
        }
    }
}
