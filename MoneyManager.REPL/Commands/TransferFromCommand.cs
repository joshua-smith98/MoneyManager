using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class TransferFromCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "from";

        public override string About => "Used with 'with' to transfer an amount from the given account to this one.";

        public override Command[] SubCommands => [
            new TransferFromWithCommand(CommandPath)
            ];

        public override Argument[] Arguments => [
            new AccountArgument("fromAccount", true)
            ];

        public override string[] RequiredArgIDs => [
            "transferValue",
            "fromAccount"
            ];

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get arguments & context
                var transferValue = (Money)args["transferValue"].Value;
                var fromAccount = (Account)args["fromAccount"].Value;

                var account = (Account)REPL.Instance.CurrentContext;

                // Create transfer
                account.TransferFrom(fromAccount, transferValue);

                Terminal.MessageSingle($"Successfully transferred {transferValue} from '{fromAccount.Name}' to the current account.");
            };
    }
}
