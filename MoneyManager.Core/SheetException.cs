namespace MoneyManager.Core
{
    /// <summary>
    /// Represents any <see cref="Exception"/> related to the <see cref="Sheet"/> class.
    /// </summary>
    /// <param name="message"></param>
    public class SheetException(string message) : Exception(message);
}
