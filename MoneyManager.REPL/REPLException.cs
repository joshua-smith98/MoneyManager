namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents any errors that occur within the MoneyManager REPL.
    /// </summary>
    /// <param name="message"></param>
    internal class REPLException(string message) : Exception(message);
}
