namespace MoneyManager.REPL.Commands
{
    internal class EnterNewAccountCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "account";

        public override string About => "Enters into a new Account with the given name to access and edit its details.";

        public override Argument[] Arguments => [
            new StringArgument("accountName", true)
            ];

        protected override Action<ArgumentValueCollection>? Action => 
            (ArgumentValueCollection args) =>
            {
                // Invoke NewAccountCommand
                new NewAccountCommand([]).Invoke(args); // We can pass the args through as is, as the required ID is the same for NewAccountCommand

                // Invoke EnterAccountCommand
                new EnterAccountCommand([]).Invoke(args);
            };
    }
}
