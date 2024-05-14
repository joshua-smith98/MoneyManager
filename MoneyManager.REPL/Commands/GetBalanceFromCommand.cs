using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetBalanceFromCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "from";

        public override string About => "Gets the total balance for the currently loaded Account or Category, for the given period from the given date.";

        public override Command[] SubCommands => [];

        public override Argument[] Arguments => [
            new DateArgument("fromDate", true)
            ];

        public override string[] RequiredArgIDs => [
            "getPeriod",
            "fromDate"
            ];

        public override string[] OptionalArgIDs => [];

        public override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get period and date
                var getPeriod = (Period)args["getPeriod"].Value;
                var fromDate = (DateOnly)args["fromDate"].Value;

                // Case: current context is Balanceable -> print balance to terminal
                if (REPL.Instance.CurrentContext is Balanceable balanceable)
                    Terminal.MessageSingle(balanceable.BalanceInfoForPeriod(fromDate, getPeriod).Balance);

                // Otherwise -> context is invalid
                else
                    throw new REPLCommandActionException($"No account or category is currently open.");
            };
    }
}
