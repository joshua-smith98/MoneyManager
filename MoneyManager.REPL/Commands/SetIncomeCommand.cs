namespace MoneyManager.REPL.Commands
{
    internal class SetIncomeCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "income";

        public override string About => "Used with 'budget to' to set the value for the current Category's Income Budget.";

        public override Command[] SubCommands => [
            new SetIncomeBudgetCommand(_commandPath)
            ];
    }
}
