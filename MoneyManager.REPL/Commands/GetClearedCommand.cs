namespace MoneyManager.REPL.Commands
{
    internal class GetClearedCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "cleared";

        public override string About => "Used to get cleared balance, income and expense information about the current context.";

        public override Command[] SubCommands => [
            new GetClearedBalanceCommand(_commandPath),
            new GetClearedBalanceFromCommand(_commandPath),
            new GetClearedIncomeCommand(_commandPath),
            new GetClearedIncomeFromCommand(_commandPath),
            new GetClearedExpensesCommand(_commandPath),
            new GetClearedExpensesFromCommand(_commandPath),
            ];
    }
}
