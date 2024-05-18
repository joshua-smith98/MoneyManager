namespace MoneyManager.REPL.Commands
{
    internal class SetCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "set";

        public override string About => "Used to set information for open Categories (namely the budgets).";

        public override Command[] SubCommands => [
            new SetIncomeCommand(_commandPath),
            new SetExpensesCommand(_commandPath),
            ];

        public override Argument[] Arguments => [
            new PeriodArgument("setPeriod")
            ];
    }
}
