using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetBudgetBalanceCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "balance";

        public override string About => "Gets the budget balance for the currently open Category, for the given period.";

        protected override string[] additionalRequiredArgIDs => ["getPeriod"];

        public override Type[] RequiredContextTypes => [typeof(Category)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get args and context
                var getPeriod = (Period)args["getPeriod"].Value;
                var category = (Category)REPL.Instance.CurrentContext;

                // Print budget value if it exists
                Terminal.MessageSingle(category.BalancedBudget?.Get(getPeriod).ToString() ?? "Neither an Income nor an Expenses Budget is set for the current Category.");
            };
    }
}
