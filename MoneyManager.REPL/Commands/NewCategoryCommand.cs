using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class NewCategoryCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "category";

        public override string About => "Creates a new Category with the given name.";

        public override Argument[] Arguments => [
            new StringArgument("categoryName", true)
            ];

        public override string[] RequiredArgIDs => ["categoryName"];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get categoryName
                string categoryName = (string)args["categoryName"].Value;

                // Create new category
                try
                {
                    REPL.Instance.CurrentAccountBook.AddCategory(new Category(categoryName));
                }
                catch (AccountBookException e) { throw new REPLCommandActionException(e.Message); } // We need to re-throw this so it gets caught by the REPL

                Terminal.MessageSingle($"Created new Category: \"{categoryName}\"");
            };
    }
}
