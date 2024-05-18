namespace MoneyManager.REPL.Commands
{
    internal class EnterNewCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "new";

        public override string About => "Used to create and enter items to access and edit their details.";

        public override Command[] SubCommands => [
            new EnterNewAccountCommand(_commandPath),    // Global Context
            new EnterNewCategoryCommand(_commandPath)    // Global Context
            ];
    }
}
