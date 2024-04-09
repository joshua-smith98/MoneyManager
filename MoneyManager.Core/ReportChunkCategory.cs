namespace MoneyManager.Core
{
    /// <summary>
    /// The part of a Report which represents a category's balance and budget info across a certain period.
    /// </summary>
    public class ReportChunkCategory
    {
        public Category? Category { get; }

        public BalanceInfo BalanceInfo { get; }
        public Money? BudgetedIncome { get; internal init; } // Use Money for the budgets here, since we'll never need to convert to another period from this class
        public Money? BudgetedExpenses { get; internal init; }
        public Money? BudgetedBalance => BudgetedIncome is not null && BudgetedExpenses is not null ?
            BudgetedIncome + BudgetedExpenses : null;
        public Money? IncomeDifference { get; internal init; }
        public Money? ExpensesDifference { get; internal init; }
        public Money? BalanceDifference => IncomeDifference is not null && ExpensesDifference is not null ?
            IncomeDifference + ExpensesDifference : null;

        public Period Period { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate => Period.GetEndDateInclusive(StartDate);

        internal ReportChunkCategory(Category? category, DateOnly startDate, Period period, BalanceInfo balanceInfo)
        {
            Category = category;
            StartDate = startDate;
            Period = period;
            BalanceInfo = balanceInfo;
        }
    }
}
