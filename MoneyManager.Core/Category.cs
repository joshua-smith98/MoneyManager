namespace MoneyManager.Core
{
    /// <summary>
    /// A named category that transactions can be assigned to, and budgeted for.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="incomeBudget"></param>
    /// <param name="expensesBudget"></param>
    public class Category(string name, Budget? incomeBudget, Budget? expensesBudget) : Balanceable
    {
        public string Name { get; set; } = name;

        public override Transaction[] Transactions => throw new NotImplementedException(); // Will be implemented once we implement Sheets

        /// <summary>
        /// The optional <see cref="Budget"/>ed income for this <see cref="Category"/>.
        /// </summary>
        public Budget? IncomeBudget { get; } = incomeBudget;
        /// <summary>
        /// The optional <see cref="Budget"/>ed expenses for this <see cref="Category"/>.
        /// </summary>
        public Budget? ExpensesBudget { get; } = expensesBudget;

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
    }
}
