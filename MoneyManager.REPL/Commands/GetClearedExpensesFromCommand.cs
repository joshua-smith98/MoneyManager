﻿using MoneyManager.Core;

namespace MoneyManager.REPL.Commands
{
    internal class GetClearedExpensesFromCommand(string pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "from";

        public override string About => "Gets the cleared expenses for the current context, for the given period from the given date.";

        public override Argument[] Arguments => [
            new DateArgument("fromDate", true)
            ];

        public override string[] RequiredArgIDs => [
            "getPeriod",
            "fromDate"
            ];

        public override Type? RequiredContextType => typeof(Balanceable);

        protected override Action<ArgumentValueCollection>? Action =>
            (ArgumentValueCollection args) =>
            {
                // Get period and date
                var getPeriod = (Period)args["getPeriod"].Value;
                var fromDate = (DateOnly)args["fromDate"].Value;

                // Get balabceable and print to terminal
                var balanceable = (Balanceable)REPL.Instance.CurrentContext;
                Terminal.MessageSingle(balanceable.BalanceInfoForPeriod(fromDate, getPeriod).ClearedExpenses);
            };
    }
}
