﻿namespace MoneyManager.REPL.Commands
{
    internal class SetIncomeBudgetCommand(Command[] pathToThisCommand) : Command(pathToThisCommand)
    {
        public override string Str => "budget";

        public override string About => "Used with 'to' to set the Income Budget for the currently open Category.";

        public override Command[] SubCommands => [
            new SetIncomeBudgetToCommand(_commandPath)
            ];
    }
}
