namespace MoneyManager.REPL.Commands
{
    internal class NewTransactionCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "transaction";

        public override string About => "Used with the 'with' command to create a new Transaction within an Account.";

        public override Command[] SubCommands => [
            new NewTransactionWithCommand(_commandPath),
            ];
    }
}
