using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class RenameCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "rename";

        public override string About => "Renames the currently open context to the given name.";

        public override Argument[] Arguments => [
            new StringArgument("newName", true)
            ];

        public override string[] RequiredArgIDs => ["newName"];

        public override Type[] RequiredContextTypes => [typeof(Account), typeof(Category)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get newName
                var newName = (string)args["newName"].Value;

                // Rename category/account
                if (REPL.Instance.CurrentContext is Account account)
                {
                    account.Name = newName;
                    Terminal.MessageSingle("Successfully renamed account.");
                }
                else if (REPL.Instance.CurrentContext is Category category)
                {
                    category.Name = newName;
                    Terminal.MessageSingle("Successfully renamed category.");
                }
            };
    }
}
