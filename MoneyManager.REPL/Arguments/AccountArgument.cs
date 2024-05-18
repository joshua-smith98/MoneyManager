using MoneyManager.Core;

namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts an <see cref="Account"/> type.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class AccountArgument(string id, bool? isRequired = null, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Account);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Search accounts for given value -> Assume no two categories have the same name
            var searchResult = REPL.Instance.CurrentAccountBook.Accounts.Where(x => x.Name.ToLower() == argumentSubStr.ToLower());

            // Check for any results
            if (!searchResult.Any())
                throw new REPLArgumentParseException($"Error parsing argument [{id}]: No account exists with name '{argumentSubStr}'");

            // Get account and return
            return new ArgumentValue(searchResult.First(), Type);
        }
    }
}
