
using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class NewTransactionWithCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "with";

        public override string About => "Creates a new Transaction with the given details within the currently open Account.";

        public override Argument[] Arguments => [
            new MoneyArgument("transactionValue", true, "value"),
            new DateArgument("transactionDate", false, "date"),
            new StringArgument("transactionPayee", false, "payee"),
            new StringArgument("transactionMemo", false, "memo"),
            new StringArgument("transactionNumber", false, "number"),
            new StringArgument("transactionCategory", false, "category")
            ];

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get arguments & context
                Money transactionValue = (Money)args["transactionValue"].Value;
                DateOnly? transactionDate = args.ContainsID("transactionDate") ? (DateOnly)args["transactionDate"].Value : null;
                string transactionPayee = args.ContainsID("transactionPayee") ? (string)args["transactionPayee"].Value : string.Empty;
                string transactionMemo = args.ContainsID("transactionMemo") ? (string)args["transactionMemo"].Value : string.Empty;
                string transactionNumber = args.ContainsID("transactionNumber") ? (string)args["transactionNumber"].Value : string.Empty;
                Category? transactionCategory = args.ContainsID("transactionCategory") ? (Category)args["transactionCategory"].Value : null;

                var account = (Account)REPL.Instance.CurrentContext;

                // Add transaction
                var transaction = transactionDate is not null
                    ? new Transaction(transactionValue, transactionDate.Value, transactionPayee, transactionMemo, transactionNumber, transactionCategory)
                    : new Transaction(transactionValue, transactionPayee, transactionMemo, transactionNumber, transactionCategory);

                Terminal.MessageSingle($"Successfully created new Transaction in '{account.Name}' Account");
            };
    }
}
