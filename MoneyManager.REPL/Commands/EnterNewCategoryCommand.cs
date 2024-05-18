
namespace MoneyManager.REPL.Commands
{
    internal class EnterNewCategoryCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "category";

        public override string About => "Enters into a new Category with the given name, to access and edit its details.";

        public override Argument[] Arguments => [
            new StringArgument("categoryName", true)
            ];

        public override string[] RequiredArgIDs => ["categoryName"];

        protected override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Invoke NewCategoryCommand
                new NewCategoryCommand([]).Invoke(args); // We can pass the args through as is, as the required ID is the same for NewCategoryCommand

                // Invoke EnterCategoryCommand
                new EnterCategoryCommand([]).Invoke(args);
            };
    }
}
