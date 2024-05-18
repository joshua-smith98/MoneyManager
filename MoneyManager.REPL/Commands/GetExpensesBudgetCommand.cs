using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetExpensesBudgetCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "budget";

        public override string About => "Gets the expenses budget for the currently open Category for the given period.";

        public override string[] RequiredArgIDs => ["getPeriod"];

        public override Type[] RequiredContextTypes => [typeof(Category)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get args and context
                var getPeriod = (Period)args["getPeriod"].Value;
                var category = (Category)REPL.Instance.CurrentContext;

                // Print budget value if it exists
                Terminal.MessageSingle(category.ExpensesBudget?.Get(getPeriod).ToString() ?? "No Expenses Budget set for current Category.");
            };
    }
}
