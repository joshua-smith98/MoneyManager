namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents any errors which occur when parsing an argument's value.
    /// </summary>
    /// <param name="message"></param>
    internal class REPLArgumentParseException(string message) : REPLException(message);
}
