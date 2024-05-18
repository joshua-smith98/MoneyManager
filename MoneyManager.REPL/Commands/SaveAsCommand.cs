
using MoneyManager.FileSystem;

namespace MoneyManager.REPL.Commands
{
    internal class SaveAsCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "as";

        public override string About => "Saves the currently loaded Account Book to the given path.";

        public override Argument[] Arguments => [
            new StringArgument("accountBookPath", true)
            ];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                var accountBookPath = (string)args["accountBookPath"].Value;

                // Validity check: directory must exist
                var dirName = Path.GetDirectoryName(accountBookPath);
                if (dirName != string.Empty && !Directory.Exists(dirName))
                    throw new REPLCommandActionException($"Directory does not exist: {dirName}");

                // Case: file already exists -> acquire overwrite permission
                if (File.Exists(accountBookPath) && !Terminal.GetUserApproval("Overwrite existing file (y/n)?"))
                    throw new REPLCommandActionException("Save cancelled by user.");

                // Otherwise, create or update file and save to path
                if (REPL.Instance.CurrentAccountBookFile is null)
                    REPL.Instance.CurrentAccountBookFile = AccountBookFile.Deconstruct(REPL.Instance.CurrentAccountBook);
                else
                    REPL.Instance.CurrentAccountBookFile.UpdateFrom(REPL.Instance.CurrentAccountBook);

                REPL.Instance.CurrentAccountBookFile.SaveTo(accountBookPath);
                Terminal.MessageSingle($"Account Book successfully saved to new file at: {accountBookPath}");
            };
    }
}
