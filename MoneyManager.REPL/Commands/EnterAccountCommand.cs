namespace MoneyManager.REPL.Commands
{
    internal class EnterAccountCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "account";

        public override string About => "Enters into the given Account to access and edit its details.";

        public override Argument[] Arguments => [
            new StringArgument("accountName", true)
            ];

        public override string[] RequiredArgIDs => ["accountName"];

        protected override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Get account name
                string accountName = (string)args["accountName"].Value;

                // Validity check: an account with the given name must exist
                if (!REPL.Instance.CurrentAccountBook.Accounts.Select(x => x.Name).Contains(accountName))
                    throw new REPLCommandActionException($"Couldn't find Account: \"{accountName}\"");

                // Enter into the context of the account
                REPL.Instance.CurrentContext = REPL.Instance.CurrentAccountBook.Accounts.Where(x => x.Name == accountName).First();
                Terminal.MessageSingle($"Successfully entered into account: {accountName}");
            };
    }
}
