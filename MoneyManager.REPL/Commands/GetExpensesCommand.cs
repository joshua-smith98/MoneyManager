using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetExpensesCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "expenses";

        public override string About => "Gets the total expenses for the current context.";

        public override Command[] SubCommands => [
            new GetIncomeFromCommand(_commandPath)
            ];

        public override Type[] RequiredContextTypes => [typeof(Balanceable)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Aquire balanceable and print expenses to terminal
                var balanceable = (Balanceable)REPL.Instance.CurrentContext;
                Terminal.MessageSingle(balanceable.BalanceInfo.Expenses);
            };
    }
}
