
namespace MoneyManager.REPL.Commands
{
    internal class DeleteCategoryCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "category";

        public override string About => "Deletes the Category with the given name, or chosen from a selection menu.";

        public override Argument[] Arguments => [
            new StringArgument("categoryName", false)
            ];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Case: categoryName is provided -> Delete category if it exists
                if (args.ContainsID("categoryName"))
                {
                    // Get categoryName
                    var categoryName = (string)args["categoryName"].Value;

                    // Validity check: category with given name must exist
                    if (!REPL.Instance.CurrentAccountBook.Categories.Select(x => x.Name).Contains(categoryName))
                        throw new REPLCommandActionException($"Couldn't find Category: \"{categoryName}\"");

                    // Reset context if required
                    var categoryToDelete = REPL.Instance.CurrentAccountBook.Categories.Where(x => x.Name == categoryName).First();
                    if (REPL.Instance.CurrentContext == categoryToDelete)
                        REPL.Instance.CurrentContext = REPL.Instance.CurrentAccountBook;

                    // Delete category
                    REPL.Instance.CurrentAccountBook.RemoveCategory(categoryToDelete);

                    Terminal.MessageSingle($"Deleted account: \"{categoryName}\"");
                }

                // Case: categoryName not provided -> Select category from menu
                else
                {
                    string[] selectionMenuItems = [.. REPL.Instance.CurrentAccountBook.Categories.Select(x => x.Name), "Cancel"];

                    int selection = Terminal.GetUserSelectionFrom(selectionMenuItems);

                    // Case: User selected last item ("Cancel") -> print message and exit action
                    if (selection == selectionMenuItems.Length - 1)
                        throw new REPLCommandActionException("Category deletion cancelled by user.");

                    // Reset context if required
                    if (REPL.Instance.CurrentContext == REPL.Instance.CurrentAccountBook.Categories[selection])
                        REPL.Instance.CurrentContext = REPL.Instance.CurrentAccountBook;

                    // Otherwise -> Delete selected category
                    REPL.Instance.CurrentAccountBook.RemoveCategoryAt(selection);
                    Terminal.MessageSingle($"Deleted category: \"{selectionMenuItems[selection]}\"");
                }
            };
    }
}
