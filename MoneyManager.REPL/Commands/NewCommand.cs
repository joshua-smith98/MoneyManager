
namespace MoneyManager.REPL.Commands
{
    internal class NewCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "new";

        public override string About => "Facilitates the creation of new items via its subcommands.";

        public override Command[] SubCommands => [
            new NewBookCommand(_commandPath),        // Global Context
            new NewAccountCommand(_commandPath),     // Global Context
            new NewCategoryCommand(_commandPath),    // Global Context
            new NewTransactionCommand(_commandPath), // Account Context
            ];
    }
}
