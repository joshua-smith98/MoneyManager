namespace MoneyManager.REPL
{
    /// <summary>
    /// Thrown when there are no matches for a user input command.
    /// </summary>
    /// <param name="message"></param>
    internal class REPLCommandNotFoundException(string message) : REPLException(message);
}
