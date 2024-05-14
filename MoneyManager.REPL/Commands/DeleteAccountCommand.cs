
namespace MoneyManager.REPL.Commands
{
    internal class DeleteAccountCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "account";

        public override string About => "Deletes the Account with the given name, or chosen from a selection menu.";

        public override Command[] SubCommands => [];

        public override Argument[] Arguments => [
            new StringArgument("accountName", false)
            ];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => ["accountName"];

        public override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Case: accountName is provided -> Delete account if it exists
                if (args.ContainsID("accountName"))
                {
                    // Get accountName
                    var accountName = (string)args["accountName"].Value;

                    // Validity check: account with given name must exist
                    if (!REPL.Instance.CurrentAccountBook.Accounts.Select(x => x.Name).Contains(accountName))
                        throw new REPLCommandActionException($"Couldn't find Account: \"{accountName}\"");

                    // Reset context if required
                    var accountToDelete = REPL.Instance.CurrentAccountBook.Accounts.Where(x => x.Name == accountName).First();
                    if (REPL.Instance.CurrentContext == accountToDelete)
                        REPL.Instance.CurrentContext = REPL.Instance.CurrentAccountBook;

                    // Delete account
                    REPL.Instance.CurrentAccountBook.RemoveAccount(accountToDelete);

                    Terminal.MessageSingle($"Deleted account: \"{accountName}\"");
                }

                // Case: accountName not provided -> Select account from menu
                else
                {
                    string[] selectionMenuItems = [.. REPL.Instance.CurrentAccountBook.Accounts.Select(x => x.Name), "Cancel"];
                    
                    int selection = Terminal.GetUserSelectionFrom(selectionMenuItems);

                    // Case: User selected last item ("Cancel") -> print message and exit action
                    if (selection == selectionMenuItems.Length - 1)
                    {
                        Terminal.MessageSingle("Account deletion cancelled by user.", ConsoleColor.Red);
                        return;
                    }

                    // Reset context if required
                    if (REPL.Instance.CurrentContext == REPL.Instance.CurrentAccountBook.Accounts[selection])
                        REPL.Instance.CurrentContext = REPL.Instance.CurrentAccountBook;

                    // Otherwise -> Delete selected account
                    REPL.Instance.CurrentAccountBook.RemoveAccountAt(selection);
                    Terminal.MessageSingle($"Deleted account: \"{selectionMenuItems[selection]}\"");
                }
            };
    }
}
