namespace MoneyManager.Core
{
    /// <summary>
    /// Represents any <see cref="Exception"/> related to the <see cref="Transaction"/> class.
    /// </summary>
    public class TransactionException(string message) : Exception(message) { }
}
