namespace MoneyManager.REPL.Commands
{
    internal class DeleteTransactionCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "transaction";

        public override string About => "Used with the 'with' command to delete Transactions.";

        public override Command[] SubCommands => [
            new DeleteTransactionWithCommand(_commandPath)
            ];
    }
}
