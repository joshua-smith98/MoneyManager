using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class TransferToCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "from";

        public override string About => "Used with 'with' to transfer an amount to the given account from this one.";

        public override Command[] SubCommands => [
            new TransferToWithCommand(_commandPath)
            ];

        public override Argument[] Arguments => [
            new AccountArgument("toAccount", true)
            ];

        protected override string[] additionalRequiredArgIDs => ["transferValue"];

        public override Type[] RequiredContextTypes => [typeof(Account)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get arguments & context
                var transferValue = (Money)args["transferValue"].Value;
                var toAccount = (Account)args["toAccount"].Value;

                var account = (Account)REPL.Instance.CurrentContext;

                // Create transfer
                account.TransferTo(toAccount, transferValue);

                Terminal.MessageSingle($"Successfully transferred {transferValue} to '{toAccount.Name}' from the current account.");
            };
    }
}
