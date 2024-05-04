namespace MoneyManager.REPL
{
    internal class StringArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(string);

        protected override ArgumentValue Parse(string argumentSubStr)
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
