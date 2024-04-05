namespace MoneyManager.Core
{
    /// <summary>
    /// A named category that transactions can be assigned to, and budgeted for.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="incomeBudget"></param>
    /// <param name="expensesBudget"></param>
    public class Category(string name, Budget? incomeBudget = null, Budget? expensesBudget = null) : Balanceable
    {
        public string Name { get; set; } = name;

        public override Transaction[] Transactions => transactions.ToArray(); // Will be implemented once we implement Sheets
        private List<Transaction> transactions = [];

        /// <summary>
        /// Gets or sets the optional <see cref="Budget"/>ed income for this <see cref="Category"/>.
        /// </summary>
        public Budget? IncomeBudget { get; set; } = incomeBudget;

        /// <summary>
        /// Gets or sets the optional <see cref="Budget"/>ed expenses for this <see cref="Category"/>.
        /// </summary>
        public Budget? ExpensesBudget { get; set; } = expensesBudget;

        /// <summary>
        /// Gets the total balance of the <see cref="IncomeBudget"/> and <see cref="ExpensesBudget"/> for this <see cref="Category"/>.
        /// </summary>
        public Budget? BalancedBudget
            => (IncomeBudget is not null && ExpensesBudget is not null)
            ? Budget.Sum(IncomeBudget, ExpensesBudget, Period.Null)
            : null;

        internal void AddTransaction(Transaction transaction)
        {
            // Since this is just internal, we won't bother with validity checks - if we get something wrong it's on us
            transactions.Add(transaction);
            transactions.Sort((x, y) => x.Date.CompareTo(y.Date));
        }

        internal void RemoveTransaction(Transaction transaction)
        {
            transactions.Remove(transaction);
        }

        /// <summary>
        /// Gets the difference between the budgeted income and the actual income for the given period, starting at the given date.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="CategoryException"></exception>
        public Money GetIncomeDifference(DateOnly from, Period period)
        {
            // Validity check: IncomeBudget must exist
            if (IncomeBudget is null) throw new CategoryException($"Tried to get income difference for category \"{Name}\", when an income budget for that category doesn't exist.");

            // Collect transactions and calculate difference
            return BalanceInfoForPeriod(from, period).Income - IncomeBudget.Get(period);
        }

        /// <summary>
        /// Gets the difference between the budgeted expenses and the actual expenses for the given period, starting at the given date.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="CategoryException"></exception>
        public Money GetExpensesDifference(DateOnly from, Period period)
        {
            // Validity check: ExpensesBudget must exist
            if (ExpensesBudget is null) throw new CategoryException($"Tried to get expenses difference for category \"{Name}\", when an expenses budget for that category doesn't exist.");

            // Collect transactions and calculate difference;
            return BalanceInfoForPeriod(from, period).Expenses - ExpensesBudget.Get(period);
        }

        /// <summary>
        /// Gets the difference between the balance of the income and expenses budget, and the actual balance for the given period, starting at the given date.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="CategoryException"></exception>
        public Money GetBalanceDifference(DateOnly from, Period period)
        {
            // Validity check: BalancedBudget must exist
            if (BalancedBudget is null) throw new CategoryException($"Tried to get expenses difference for category \"{Name}\", when either an income or expenses budget for that category doesn't exist.");

            // Collect transactions and calculate difference;
            return BalanceInfoForPeriod(from, period).Balance - BalancedBudget.Get(period);
        }
    }
}
