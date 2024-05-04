using MoneyManager.Core;

namespace MoneyManager.REPL
{
    internal class PeriodArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Period);

        protected override ArgumentValue Parse(string argumentSubStr)
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
