namespace MoneyManager.REPL
{
    /// <summary>
    /// Thrown when there are no matches for a user input argument.
    /// </summary>
    /// <param name="message"></param>
    internal class REPLArgumentNotFoundException(string message) : REPLException(message);
}
