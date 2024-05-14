namespace MoneyManager.REPL.Commands
{
    internal class GetCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "get";

        public override string About => "Used to get information about the current context.";

        public override Command[] SubCommands => [
            new GetBalanceCommand(CommandPath), // Balanceable Context
            new GetIncomeCommand(CommandPath), // Balanceable/Category Context
            new GetExpensesCommand(CommandPath), // Balanceable/Category Context
            new GetClearedCommand(CommandPath), // Balanceable Context
            ];

        public override Argument[] Arguments => [
            new PeriodArgument("getPeriod", false)
            ];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => null;
    }
}
