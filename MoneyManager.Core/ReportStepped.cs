namespace MoneyManager.Core
{
    /// <summary>
    /// A Report with periodic "steps" across a certain period, each containing balance info across its step period.
    /// </summary>
    public class ReportStepped
    {
        /// <summary>
        /// Contains the information about each "step" of this report.
        /// </summary>
        public ReportChunk[] ReportChunks { get; }

        /// <summary>
        /// The <see cref="ReportChunk"/> that contains information about the whole period of this Report.
        /// </summary>
        public ReportChunk TotalReportChunk { get; }

        public Period Period { get; }
        public Period StepPeriod { get; }
        public DateOnly StartDate { get; }
        public DateOnly EndDate => ReportChunks.Last().EndDate; // In case the stepPeriod doesn't divide evenly into the period

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
