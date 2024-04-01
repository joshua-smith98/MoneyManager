namespace MoneyManager.Core
{
    public class Category(string name, Budget? incomeBudget, Budget? expensesBudget) : Balanceable
    {
        public string Name { get; set; } = name;

        public override Transaction[] Transactions => throw new NotImplementedException(); // Will be implemented once we implement Sheets

        public Budget? IncomeBudget { get; } = incomeBudget;
        public Budget? ExpensesBudget { get; } = expensesBudget;

        public Money GetIncomeDifference(DateOnly from, Period period)
        {
            // Validity check: IncomeBudget must exist
            if (IncomeBudget is null) throw new CategoryException($"Tried to get income difference for category \"{Name}\", when an income budget for that category doesn't exist.");

            var to = GetToDate(from, period);
            var transactionsBetween = Transactions.Where(x => x.Date >= from && x.Date <= to);
            var actualIncome = BalanceInfoBetween(transactionsBetween.First(), transactionsBetween.Last()).Income;
            return actualIncome - IncomeBudget.Get(period);
        }

        public Money GetExpensesDifference(DateOnly from, Period period)
        {
            // Validity check: ExpensesBudget must exist
            if (ExpensesBudget is null) throw new CategoryException($"Tried to get expenses difference for category \"{Name}\", when an expenses budget for that category doesn't exist.");

            var to = GetToDate(from, period);
            var transactionsBetween = Transactions.Where(x => x.Date >= from && x.Date <= to);
            var actualExpenses = BalanceInfoBetween(transactionsBetween.First(), transactionsBetween.Last()).Expenses;
            return actualExpenses - ExpensesBudget.Get(period);
        }

        private static DateOnly GetToDate(DateOnly from, Period period)
        {
            // Get the date the period extends to, careful to be inclusive
            switch (period)
            {
                case Period.Daily:
                    return from;
                case Period.Weekly:
                    return from.AddDays(6);
                case Period.Fortnightly:
                    return from.AddDays(15);
                case Period.Monthly:
                    return from.AddMonths(1).AddDays(-1);
                case Period.Quarterly:
                    return from.AddMonths(3).AddDays(-1);
                case Period.Biannually:
                    return from.AddMonths(6).AddDays(-1);
                case Period.Annually:
                    return from.AddYears(1).AddDays(-1);
                default:
                    throw new CategoryException("Period must not be null!");
            }
        }
    }
}
