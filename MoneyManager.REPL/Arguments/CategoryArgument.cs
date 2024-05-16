using MoneyManager.Core;

namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an <see cref="Argument"/> which accepts a <see cref="Category"/> type.
    /// </summary>
    /// <param name="id">The internal identifier for this argument instance.</param>
    /// <param name="isRequired">[Deprecated]</param>
    /// <param name="str">The string the user uses to refer to this argument in a command path.</param>
    internal class CategoryArgument(string id, bool isRequired, string? str = null) : Argument(id, isRequired, str)
    {
        public override Type Type => typeof(Category);

        protected override ArgumentValue ParseImpl(string argumentSubStr)
        {
            // Remove ending comma or semi-colon if it exists
            if (argumentSubStr[^1] == ',' || argumentSubStr[^1] == ';')
                argumentSubStr = argumentSubStr[..^1];

            // Search categories for given value -> Assume no two categories have the same name
            var searchResult = REPL.Instance.CurrentAccountBook.Categories.Where(x => x.Name.ToLower() == argumentSubStr.ToLower());

            // Check for any results
            if (!searchResult.Any())
                throw new REPLArgumentParseException($"Error parsing argument [{id}]: No category exists with name '{argumentSubStr}'");

            // Get category and return
            return new ArgumentValue(searchResult.First(), Type);
        }
    }
}
