namespace MoneyManager.REPL.Commands
{
    internal class GetCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "get";

        public override string About => "Used to get information about the current context.";

        public override Command[] SubCommands => [
            new GetBalanceCommand(_commandPath), // Balanceable Context
            new GetIncomeCommand(_commandPath), // Balanceable/Category Context
            new GetExpensesCommand(_commandPath), // Balanceable/Category Context
            new GetClearedCommand(_commandPath), // Balanceable Context
            ];

        public override Argument[] Arguments => [
            new PeriodArgument("getPeriod")
            ];
    }
}
