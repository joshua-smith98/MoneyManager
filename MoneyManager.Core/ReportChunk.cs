using System.Collections.ObjectModel;

namespace MoneyManager.Core
{
    public class ReportChunk
    {
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
