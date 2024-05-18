using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class SetIncomeBudgetToCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "to";

        public override string About => "Set the Income Budget of the currently open Category to the given value.";

        public override Argument[] Arguments => [
            new MoneyArgument("budgetValue", true)
            ];

        protected override string[] additionalRequiredArgIDs => ["setPeriod"];

        public override Type[] RequiredContextTypes => [typeof(Category)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get args and context
                var setPeriod = (Period)args["setPeriod"].Value;
                var budgetValue = (Money)args["budgetValue"].Value;
                var category = (Category)REPL.Instance.CurrentContext;

                // Remove budget if set to zero
                if (budgetValue == 0)
                {
                    category.IncomeBudget = null;
                    Terminal.MessageSingle("Successfully removed Income Budget from the current Category.");
                    return;
                }

                // Set/Initialise budget as required
                if (category.IncomeBudget is not null)
                    category.IncomeBudget.Set(budgetValue, setPeriod);
                else
                    category.IncomeBudget = new Budget(budgetValue, setPeriod);

                Terminal.MessageSingle($"Successfully set Income Budget for the current Category to {budgetValue}, {setPeriod}.");
            };
    }
}
