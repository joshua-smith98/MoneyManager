namespace MoneyManager.Core
{
    /// <summary>
    /// Represents a cyclic period of time. Used in <see cref="Budget"/>s and reports.
    /// </summary>
    public enum Period
    {
        Null,
        Daily,
        Weekly,
        Fortnightly,
        Monthly,
        Quarterly,
        Biannually,
        Annually
    }
}
