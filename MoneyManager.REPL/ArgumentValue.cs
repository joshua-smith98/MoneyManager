namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents a value parsed by an <see cref="Argument"/> from a substring of a command path.
    /// Contains a generic <see cref="object"/> value, along with its <see cref="System.Type"/>.
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Type"></param>
    internal record ArgumentValue(object Value, Type Type);
}
