namespace MoneyManager.REPL.Commands
{
    internal class TransferCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "transfer";

        public override string About => "Used with 'to' and 'from' to create transfers between accounts.";

        public override Command[] SubCommands => [
            new TransferToCommand(_commandPath),
            new TransferFromCommand(_commandPath)
            ];

        public override Argument[] Arguments => [
            new MoneyArgument("transferValue")
            ];
    }
}
