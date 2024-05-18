namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts a <see cref="DateOnly"/> type.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class DateArgument(string id, bool? isRequired = null, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(DateOnly);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Parse date
            if (!DateOnly.TryParse(argumentSubStr, out DateOnly parsedValue))
                throw new REPLArgumentParseException($"Date provided for [{ID}] was of an invalid format.");

            // Construct and return
            return new ArgumentValue(parsedValue, Type);
        }
    }
}
