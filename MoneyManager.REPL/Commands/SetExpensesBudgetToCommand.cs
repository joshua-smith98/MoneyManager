using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class SetExpensesBudgetToCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "to";

        public override string About => "Set the Expenses Budget of the currently open Category to the given value.";

        public override Argument[] Arguments => [
            new MoneyArgument("budgetValue", true)
            ];

        public override string[] RequiredArgIDs => [
            "setPeriod",
            "budgetValue"
            ];

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
                    category.ExpensesBudget = null;
                    Terminal.MessageSingle("Successfully removed Expenses Budget from the current Category.");
                    return;
                }

                // Set/Initialise budget as required
                if (category.ExpensesBudget is not null)
                    category.ExpensesBudget.Set(budgetValue, setPeriod);
                else
                    category.ExpensesBudget = new Budget(budgetValue, setPeriod);

                Terminal.MessageSingle($"Successfully set Expenses Budget for the current Category to {budgetValue}, {setPeriod}.");
            };
    }
}
