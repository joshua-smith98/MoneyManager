using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetBalanceCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "balance";

        public override string About => "Gets the total balance for the currently open Account or Category.";

        public override Command[] SubCommands => [
            new GetBalanceFromCommand(CommandPath)
            ];

        public override Argument[] Arguments => [];

        public override string[] RequiredArgIDs => [];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Case: current context is Balanceable -> print balance to terminal
                if (REPL.Instance.CurrentContext is Balanceable balanceable)
                    Terminal.MessageSingle(balanceable.BalanceInfo.Balance);
                
                // Otherwise -> context is invalid
                else
                    throw new REPLCommandActionException($"No account or category is currently open.");
            };
    }
}
