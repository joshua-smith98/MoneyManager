
namespace MoneyManager.REPL.Commands
{
    internal class EnterCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "enter";

        public override string About => "Used to enter items to access and edit their details.";

        public override Command[] SubCommands => [
            new EnterAccountCommand(_commandPath),   // Global Context
            new EnterCategoryCommand(_commandPath),  // Global Context
            new EnterNewCommand(_commandPath)        // Global Context
            ];
    }
}
