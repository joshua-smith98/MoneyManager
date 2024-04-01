namespace MoneyManager.Core
{
    public class Budget
    {
        private Money? perDay;
        private Money? perWeek;
        private Money? perFortnight;
        private Money? perMonth;
        private Money? perQuarter;
        private Money? per6Months;
        private Money? perYear;

        public Money PerDay
        {
            get
            {

            }
            set
            {

            }
        }

        public Money PerWeek
        {
            get
            {

            }
            set
            {

            }
        }

        public Money PerFortnight
        {
            get
            {

            }
            set
            {

            }
        }

        public Money PerMonth
        {
            get
            {

            }
            set
            {

            }
        }

        public Money PerQuarter
        {
            get
            {

            }
            set
            {

            }
        }

        public Money Per6Months
        {
            get
            {

            }
            set
            {

            }
        }

        public Money PerYear
        {
            get
            {

            }
            set
            {

            }
        }

        public Period CurrentPeriod { get; private set; }

        public Budget(Money value, Period period)
        {

        }
    }
}
