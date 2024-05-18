
namespace MoneyManager.REPL.Commands
{
    internal class DeleteCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "delete";

        public override string About => "Used to delete items.";

        public override Command[] SubCommands => [
            new DeleteAccountCommand(_commandPath),      // Global Context
            new DeleteCategoryCommand(_commandPath),     // Global Context
            new DeleteTransactionCommand(_commandPath),  // Account Context
            ];
    }
}
