namespace MoneyManager.REPL.Commands
{
    internal class GetClearedCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "cleared";

        public override string About => "Used to get cleared balance, income and expense information about the current context.";

        public override Command[] SubCommands => [
            new GetClearedBalanceCommand(CommandPath),
            new GetClearedBalanceFromCommand(CommandPath),
            new GetClearedIncomeCommand(CommandPath),
            new GetClearedIncomeFromCommand(CommandPath),
            new GetClearedExpensesCommand(CommandPath),
            new GetClearedExpensesFromCommand(CommandPath),
            ];
    }
}
