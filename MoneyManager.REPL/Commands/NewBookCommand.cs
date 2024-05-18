using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class NewBookCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "book";

        public override string About => "Creates a new Account Book.";

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Confirm whether to save current account book or not
                switch (Terminal.GetUserApprovalOrCancel("Do you want to save the currently open Account Book first (y/n/c)? "))
                {
                    // Case: user says yes -> invoke save command
                    case Terminal.UserResult.Yes:
                        // Run "save book" command
                        new SaveCommand([]).Invoke([]);
                        break;

                    // Case: user says no -> continue without saving
                    case Terminal.UserResult.No: break;

                    // Case: user cancels or gives any other response -> print message and exit action
                    default:
                        throw new REPLCommandActionException("Save cancelled by user.");
                }

                // Create new AccountBook and reset file
                REPL.Instance.CurrentAccountBook = new AccountBook();
                REPL.Instance.CurrentAccountBookFile = null;
            };
    }
}
