
namespace MoneyManager.REPL.Commands
{
    internal class NewCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "new";

        public override string About => "Facilitates the creation of new items via its subcommands.";

        public override Command[] SubCommands => [
            new NewBookCommand(CommandPath),        // Global Context
            new NewAccountCommand(CommandPath),     // Global Context
            new NewCategoryCommand(CommandPath),    // Global Context
            new NewTransactionCommand(CommandPath), // Account Context
            ];

        public override Argument[] Arguments => [];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => null;
    }
}
