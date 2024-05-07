namespace MoneyManager.REPL.Commands
{
    internal class EnterNewAccountCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "account";

        public override string About => "Enters into a new Account with the given name to access and edit its details.";

        public override Command[] SubCommands => [];

        public override Argument[] Arguments => [
            new StringArgument("accountName", true)
            ];

        public override string[] RequiredArgIDs => ["accountName"];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Invoke NewAccountCommand
                new NewAccountCommand("").Action!(args); // We can pass the args through as is, as the required ID is the same for NewAccountCommand

                // Invoke EnterAccountCommand
                new EnterAccountCommand("").Action!(args);
            };
    }
}
