using MoneyManager.Core;
using System.Globalization;

namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts a <see cref="Money"/> type.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class MoneyArgument(string id, bool? isRequired = null, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Money);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
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
