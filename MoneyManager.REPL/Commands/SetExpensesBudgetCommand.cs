namespace MoneyManager.REPL.Commands
{
    internal class SetExpensesBudgetCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "budget";

        public override string About => "Used with 'to' to set the Expenses Budget for the currently open Category.";

        public override Command[] SubCommands => [
            new SetExpensesBudgetToCommand(CommandPath)
            ];
    }
}
