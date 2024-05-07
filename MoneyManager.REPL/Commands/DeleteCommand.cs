
namespace MoneyManager.REPL.Commands
{
    internal class DeleteCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "delete";

        public override string About => "Used to delete items.";

        public override Command[] SubCommands => [
            new DeleteAccountCommand(CommandPath),      // Global Context
            new DeleteCategoryCommand(CommandPath),     // Global Context
            new DeleteTransactionCommand(CommandPath),  // Account Context
            new DeleteIncomeCommand(CommandPath),       // Category Context
            new DeleteExpensesCommand(CommandPath)      // Category Context
            ];

        public override Argument[] Arguments => [];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => null;
    }
}
