using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetIncomeCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "income";

        public override string About => "Gets the total income for the current context.";

        public override Command[] SubCommands => [
            new GetIncomeFromCommand(CommandPath)
            ];

        public override Type? RequiredContextType => typeof(Balanceable);

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Aquire balanceable and print income to terminal
                var balanceable = (Balanceable)REPL.Instance.CurrentContext;
                Terminal.MessageSingle(balanceable.BalanceInfo.Income);
            };
    }
}
