using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetClearedExpensesCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "expenses";

        public override string About => "Gets the cleared expenses for the current context.";

        public override Command[] SubCommands => [
            new GetIncomeFromCommand(CommandPath)
            ];

        public override Type[] RequiredContextTypes => [typeof(Balanceable)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Aquire balanceable and print expenses to terminal
                var balanceable = (Balanceable)REPL.Instance.CurrentContext;
                Terminal.MessageSingle(balanceable.BalanceInfo.ClearedExpenses);
            };
    }
}
