
namespace MoneyManager.REPL.Commands
{
    internal class ExitProgramCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "program";

        public override string About => "Exits the program.";

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Confirm exit of program, save account book and stop REPL loop
                switch (Terminal.GetUserApprovalOrCancel("Do you want to save the currently open Account Book first (y/n/c)? "))
                {
                    // Case: user says yes -> invoke save command
                    case Terminal.UserResult.Yes:
                        // Run "save book" command
                        new SaveCommand("").Action!([]); // If user cancells inside SaveCommand, then the REPL will catch the exception and we won't continue
                        break;

                    // Case: user says no -> continue without saving
                    case Terminal.UserResult.No: break;

                    // Case: user cancels or gives any other response -> print message and exit action
                    default:
                        throw new REPLCommandActionException("Exit cancelled by user.");
                }

                // If we've got this far, then user has approved exit of program, and we'll stop the REPL now
                REPL.Instance.IsRunning = false;
            };
    }
}
