using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class DeleteTransactionWithCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "with";

        public override string About => "Lists all transactions with the given criteria, and deletes the selected one.";

        public override Argument[] Arguments => [
            new MoneyArgument("transactionValue", false, "value"),
            new DateArgument("transactionDate", false, "date"),
            new StringArgument("transactionPayee", false, "payee"),
            new StringArgument("transactionMemo", false, "memo"),
            new StringArgument("transactionNumber", false, "number"),
            new StringArgument("transactionCategory", false, "category")
            ];

        public override string[] OptionalArgIDs => [
            "transactionValue",
            "transactionDate",
            "transactionPayee",
            "transactionMemo",
            "transactionNumber",
            "transactionCategory"
            ];

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get arguments
                Money? transactionValue = args.ContainsID("transactionDate") ? (Money)args["transactionValue"].Value : null;
                DateOnly? transactionDate = args.ContainsID("transactionDate") ? (DateOnly)args["transactionDate"].Value : null;
                string transactionPayee = args.ContainsID("transactionPayee") ? (string)args["transactionPayee"].Value : string.Empty;
                string transactionMemo = args.ContainsID("transactionMemo") ? (string)args["transactionMemo"].Value : string.Empty;
                string transactionNumber = args.ContainsID("transactionNumber") ? (string)args["transactionNumber"].Value : string.Empty;
                Category? transactionCategory = args.ContainsID("transactionCategory") ? (Category)args["transactionCategory"].Value : null;

                var account = (Account)REPL.Instance.CurrentContext;

                // Find transactions
                var transactions = account.Transactions.Where(x => 
                    (transactionValue is not null && transactionValue == x.Value) &&
                    (transactionDate is not null && transactionDate == x.Date) &&
                    (transactionPayee is not null && transactionPayee == x.Payee) &&
                    (transactionMemo is not null && transactionMemo == x.Memo) &&
                    (transactionNumber is not null && transactionNumber == x.Number) &&
                    (transactionCategory is not null && transactionCategory == x.Category)
                );

                // Give error if no transactions match criteria
                if (!transactions.Any())
                    throw new REPLCommandActionException("No transactions match the given criteria.");

                // Display transaction selection
                string[] selectionMenuItems = [.. transactions.Select(x => $"{x.Value} | {x.Payee} | {x.Memo} | {x.Date}"), "Cancel"];
                int selection = Terminal.GetUserSelectionFrom(selectionMenuItems);

                // Handle cancel
                if (selection == selectionMenuItems.Length)
                    throw new REPLCommandActionException("Deletion cancelled by user.");

                // Delete selection
                account.DeleteTransaction(transactions.ToArray()[selection]);
                Terminal.MessageSingle("Successfully deleted 1 transaction.");
            };
    }
}
