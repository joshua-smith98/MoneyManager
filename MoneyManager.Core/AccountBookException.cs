namespace MoneyManager.Core
{
    /// <summary>
    /// Represents any <see cref="Exception"/> related to the <see cref="AccountBook"/> class.
    /// </summary>
    /// <param name="message"></param>
    public class AccountBookException(string message) : Exception(message);
}
