using MoneyManager.Core;

namespace MoneyManager.REPL
{

    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts a <see cref="Period"/> type.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class PeriodArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Period);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Check if the substr matches some Period, case insensitive -> Return if so
            var periodValues = Enum.GetValues(typeof(Period));
            foreach (var period in periodValues)
                if (Enum.GetName(typeof(Period), period)!.ToLower() == argumentSubStr.ToLower())
                    return new ArgumentValue(period, Type);

            // Otherwise a valid period wasn't given
            throw new REPLArgumentParseException($"Period provided for [{ID}] (\"{argumentSubStr}\") was invalid.");
        }
    }
}
