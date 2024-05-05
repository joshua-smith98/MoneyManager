namespace MoneyManager.REPL
{
    internal class DateArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(DateOnly);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Parse date
            DateOnly parsedValue;
            if (!DateOnly.TryParse(argumentSubStr, out parsedValue))
                throw new REPLArgumentParseException($"Date provided for [{ID}] was of an invalid format.");

            // Construct and return
            return new ArgumentValue(parsedValue, Type);
        }
    }
}
