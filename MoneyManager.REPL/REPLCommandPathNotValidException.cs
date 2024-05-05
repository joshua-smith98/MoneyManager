namespace MoneyManager.REPL
{
    /// <summary>
    /// Thrown when a user input command path doesn't end with a command containing an action.
    /// </summary>
    /// <param name="message"></param>
    internal class REPLCommandPathNotValidException(string message) : REPLException(message);
}
