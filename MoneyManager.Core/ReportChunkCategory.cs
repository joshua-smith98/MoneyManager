namespace MoneyManager.Core
{
    public class ReportChunkCategory
    {
        public Category? Category { get; }

        public BalanceInfo BalanceInfo { get; }
        public Budget? IncomeBudget { get; internal init; }
        public Budget? ExpensesBudget { get; internal init; }
        public Budget? BalancedBudget => IncomeBudget is not null && ExpensesBudget is not null ?
            Budget.Sum(IncomeBudget, ExpensesBudget, Period.Null): null;
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
