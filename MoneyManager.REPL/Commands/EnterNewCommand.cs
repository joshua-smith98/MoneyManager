namespace MoneyManager.REPL.Commands
{
    internal class EnterNewCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "new";

        public override string About => "Used to create and enter items to access and edit their details.";

        public override Command[] SubCommands => [
            new EnterNewAccountCommand(CommandPath),    // Global Context
            new EnterNewCategoryCommand(CommandPath)    // Global Context
            ];
    }
}
