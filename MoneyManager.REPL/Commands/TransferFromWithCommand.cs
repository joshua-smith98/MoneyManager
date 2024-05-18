using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class TransferFromWithCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "with";

        public override string About => "Transfers the given amount from the given account to the currently open one, applying the given details to the Transaction.";

        public override Argument[] Arguments => [
            new DateArgument("transferDate", false, "date"),
            new StringArgument("transferMemo", false, "memo"),
            new StringArgument("transferNumber", false, "number"),
            new StringArgument("transferCategory", false, "category")
            ];

        public override string[] RequiredArgIDs => [
            "transferValue",
            "fromAccount"
            ];

        public override string[] OptionalArgIDs => [
            "transferDate",
            "transferMemo",
            "transferNumber",
            "transferCategory"
            ];

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get arguments & context
                Account fromAccount = (Account)args["fromAccount"].Value;
                Money transferValue = (Money)args["transferValue"].Value;
                DateOnly? transferDate = args.ContainsID("transferDate") ? (DateOnly)args["transferDate"].Value : null;
                string transferMemo = args.ContainsID("transferMemo") ? (string)args["transferMemo"].Value : string.Empty;
                string transferNumber = args.ContainsID("transferNumber") ? (string)args["transferNumber"].Value : string.Empty;
                Category? transferCategory = args.ContainsID("transferCategory") ? (Category)args["transferCategory"].Value : null;

                var account = (Account)REPL.Instance.CurrentContext;

                // Create transfer
                if (transferDate is null)
                    account.TransferFrom(fromAccount, transferValue, transferMemo, transferNumber, transferCategory);
                else
                    account.TransferFrom(fromAccount, transferValue, transferDate.Value, transferMemo, transferNumber, transferCategory);

                Terminal.MessageSingle($"Successfully transferred {transferValue} from '{fromAccount.Name}' to the current account.");
            };
    }
}
