namespace MoneyManager.REPL.Commands
{
    internal class SetExpensesCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "expenses";

        public override string About => "Used with 'budget to' to set the value for the current Category's Expenses Budget.";

        public override Command[] SubCommands => [
            new SetExpensesBudgetCommand(_commandPath)
            ];
    }
}