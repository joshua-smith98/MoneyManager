namespace MoneyManager.REPL.Commands
{
    internal class EditCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "edit";

        public override string About => "Used to edit items.";

        public override Command[] SubCommands => [
            new EditTransactionsCommand(CommandPath)
            ];
    }
}
