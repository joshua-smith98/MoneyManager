namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts a <see cref="string"/> type, optionally surrounded by double-quotes.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class StringArgument(string id, bool? isRequired = null, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(string);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Remove any double quotes if they exist
            argumentSubStr = argumentSubStr.Replace("\"", "");

            // Construct and return value
            return new ArgumentValue(argumentSubStr, Type);
        }
    }
}
