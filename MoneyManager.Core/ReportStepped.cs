namespace MoneyManager.Core
{
    public class ReportStepped
    {
        public ReportChunk[] ReportChunks { get; }
        public ReportChunk TotalReportChunk { get; }

        public Period Period { get; }
        public Period StepPeriod { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate => Period.GetEndDateInclusive(StartDate);

        internal ReportStepped(DateOnly startDate, Period period, Period stepPeriod, ReportChunk[] reportChunks, ReportChunk totalReportChunk)
        {
            StartDate = startDate;
            Period = period;
            StepPeriod = stepPeriod;
            ReportChunks = reportChunks;
            TotalReportChunk = totalReportChunk;
        }
    }
}
