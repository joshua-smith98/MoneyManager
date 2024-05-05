namespace MoneyManager.REPL
{
    /// <summary>
    /// Thrown if some conflict is found between Argument IDs/Strs or Command Strs. This means I've made a mistake in code, and should never be thrown in a release binary.<br/>
    /// </summary>
    /// <param name="message"></param>
    internal class REPLSemanticErrorException(string message) : REPLException(message);
}
