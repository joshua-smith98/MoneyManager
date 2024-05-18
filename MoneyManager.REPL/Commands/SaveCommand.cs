
namespace MoneyManager.REPL.Commands
{
    internal class SaveCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "save";

        public override string About => "Saves the currently loaded Account Book.";

        public override Command[] SubCommands => [
            new SaveAsCommand(_commandPath)
            ];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Case: AccountBook has been saved previously
                if (REPL.Instance.CurrentAccountBookFile is not null && REPL.Instance.CurrentAccountBookFile.Path is not null)
                {
                    // Update and save
                    REPL.Instance.CurrentAccountBookFile.UpdateFrom(REPL.Instance.CurrentAccountBook);
                    REPL.Instance.CurrentAccountBookFile.SaveTo(REPL.Instance.CurrentAccountBookFile.Path);
                    Terminal.MessageSingle("Account Book Successfully saved.");
                }

                // Case: AccountBook has no previous file to save to -> acquire new path and save
                else
                {
                    // Get new path from user
                    string? newPath = Terminal.PromptCancellable("Path to save Account Book to (or ESC to cancel): ", ConsoleKey.Escape);
                    if (newPath is null)
                        throw new REPLCommandActionException("Save cancelled by user.");

                    // Invoke SaveAsCommand
                    new SaveAsCommand([]).Invoke(
                        new ArgumentValueCollection(
                            ("accountBookPath", new ArgumentValue(newPath, typeof(string)))
                            )
                        );
                }
            };
    }
}
