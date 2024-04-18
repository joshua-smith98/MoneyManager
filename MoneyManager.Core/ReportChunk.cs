namespace MoneyManager.Core
{
    /// <summary>
    /// The part of a report which represents the balance info across a certain period across all accounts and categories.
    /// </summary>
    public class ReportChunk
    {
        /// <summary>
        /// Gets the balance and budget info associated with a specific <see cref="Category"/>.
        /// </summary>
        public ReportChunkCategoryCollection ReportChunkCategories { get; }

        public BalanceInfo BalanceInfo { get; }
        public BalanceInfo BalanceInfoToDate { get; }

        public Period Period { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate => Period.GetEndDateInclusive(StartDate);

        internal ReportChunk(DateOnly startDate, Period period, BalanceInfo balanceInfo, BalanceInfo balanceInfoToDate, ReportChunkCategory[] reportChunkCategories)
        {
            StartDate = startDate;
            Period = period;
            BalanceInfo = balanceInfo;
            BalanceInfoToDate = balanceInfoToDate;
            ReportChunkCategories = new ReportChunkCategoryCollection(reportChunkCategories);
        }
    }
}
