using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class EditTransactionsCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "transactions";

        public override string About => "Opens a transaction table editor for quick and advanced editing of transactions.";

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Translate transactions into Terminal Table Info


                // Open Terminal Table Editor


                // Translate Terminal Table Info into changes made to account transactions
                // We can either do this with a diff, or we can simply create a new account with the same metadata
                // Option B is easier and more straightforward, but option A is cooler...
            };
    }
}
