using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class NewAccountCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "account";

        public override string About => "Creates a new Account with the given name.";

        public override Command[] SubCommands => [];

        public override Argument[] Arguments => [
            new StringArgument("accountName", true)
            ];

        public override string[] RequiredArgIDs => ["accountName"];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get accountName
                string accountName = (string)args["accountName"].Value;

                // Create new account
                try
                {
                    REPL.Instance.CurrentAccountBook.AddAccount(new Account(accountName));
                }
                catch (AccountBookException e) { throw new REPLCommandActionException(e.Message); } // We need to re-throw this so it gets caught by the REPL

                Terminal.MessageSingle($"Created new Account: \"{accountName}\"");
            };
    }
}
