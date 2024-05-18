namespace MoneyManager.REPL.Commands
{
    internal class SetCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "set";

        public override string About => "Used to set information for open Categories (namely the budgets).";

        public override Command[] SubCommands => [
            new SetIncomeCommand(CommandPath),
            new SetExpensesCommand(CommandPath),
            ];

        public override Argument[] Arguments => [
            new PeriodArgument("setPeriod", true)
            ];
    }
}
