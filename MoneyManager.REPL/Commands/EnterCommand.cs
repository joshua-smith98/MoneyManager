
namespace MoneyManager.REPL.Commands
{
    internal class EnterCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "enter";

        public override string About => "Used to enter items to access and edit their details.";

        public override Command[] SubCommands => [
            new EnterAccountCommand(CommandPath),   // Global Context
            new EnterCategoryCommand(CommandPath),  // Global Context
            new EnterNewCommand(CommandPath)        // Global Context
            ];

        public override Argument[] Arguments => [];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => null;
    }
}
