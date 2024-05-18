namespace MoneyManager.REPL.Commands
{
    internal class GetBudgetCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "budget";

        public override string About => "Used with 'balance' to get the balance of the income and expenses budgets for the currently open Category.";

        public override Command[] SubCommands => [
            new GetBudgetBalanceCommand(CommandPath)
            ];
    }
}
