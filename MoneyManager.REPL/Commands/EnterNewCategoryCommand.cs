﻿
namespace MoneyManager.REPL.Commands
{
    internal class EnterNewCategoryCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "category";

        public override string About => "Enters into a new Category with the given name, to access and edit its details.";

        public override Command[] SubCommands => [];

        public override Argument[] Arguments => [
            new StringArgument("categoryName", true)
            ];

        public override string[] RequiredArgIDs => ["categoryName"];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Invoke NewCategoryCommand
                new NewCategoryCommand("").Action!(args); // We can pass the args through as is, as the required ID is the same for NewCategoryCommand

                // Invoke EnterCategoryCommand
                new EnterCategoryCommand("").Action!(args);
            };
    }
}