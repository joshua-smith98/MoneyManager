namespace MoneyManager.Core
{
    /// <summary>
    /// Exception thrown when a <see cref="Transaction"/> that is invalid or of the incorrect sub-type is added to an <see cref="Account"/>.
    /// </summary>
    public class TransactionInvalidException(string message) : TransactionException(message) { }
}
