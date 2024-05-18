using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetIncomeBudgetCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "budget";

        public override string About => "Gets the income budget for the currently open Category for the given period.";

        public override string[] RequiredArgIDs => ["getPeriod"];

        public override Type[] RequiredContextTypes => [typeof(Category)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get args and context
                var getPeriod = (Period)args["getPeriod"].Value;
                var category = (Category)REPL.Instance.CurrentContext;

                // Print budget value if it exists
                Terminal.MessageSingle(category.IncomeBudget?.Get(getPeriod).ToString() ?? "No Income Budget set for current Category.");
            };
    }
}
