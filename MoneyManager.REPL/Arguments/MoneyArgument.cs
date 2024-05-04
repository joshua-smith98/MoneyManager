using MoneyManager.Core;
using System.Globalization;

namespace MoneyManager.REPL
{
    internal class MoneyArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Money);

        protected override ArgumentValue Parse(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Parse currency
            decimal parseValue;
            if (!decimal.TryParse(argumentSubStr, NumberStyles.Currency, CultureInfo.CurrentCulture, out parseValue))
                throw new REPLArgumentParseException($"Money value provided for [{ID}] was of an invalid format.");

            // Build and return
            return new ArgumentValue((Money)parseValue, Type);
        }
    }
}
