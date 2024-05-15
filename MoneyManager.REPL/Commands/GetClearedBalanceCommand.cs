using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetClearedBalanceCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "balance";

        public override string About => "Gets the cleared balance for the current context.";

        public override Command[] SubCommands => [
            new GetBalanceFromCommand(CommandPath)
            ];

        public override Type[] RequiredContextTypes => [typeof(Balanceable)];

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Aquire balanceable and print balance to terminal
                var balanceable = (Balanceable)REPL.Instance.CurrentContext;
                Terminal.MessageSingle(balanceable.BalanceInfo.ClearedBalance);
            };
    }
}
