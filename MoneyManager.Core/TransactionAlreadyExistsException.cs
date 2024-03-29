namespace MoneyManager.Core
{
    /// <summary>
    /// Exception thrown when an <see cref="Account"/> attempts to add a <see cref="Transaction"/> to itself twice.
    /// </summary>
    internal class TransactionAlreadyExistsException(string message) : TransactionException(message) { }
}
