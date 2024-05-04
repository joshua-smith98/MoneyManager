namespace MoneyManager.REPL
{
    internal abstract class Command
    {
        public abstract string Str { get; }

        public abstract string About { get; }
        public string CommandPath
        {
            get
            {
                // Build CommandPath
            }
        }

        public abstract Command[] SubCommands { get; }
        public abstract Argument[] Arguments { get; }

        public abstract Action<ArgumentValueCollection>? Action { get; }

        private string _pathToThisCommand;

        public Command(string pathToThisCommand)
        {
            // Validate constraints

            
            _pathToThisCommand = pathToThisCommand;
        }

        public bool MatchStr(string commandSubStr)
            => commandSubStr.Split().First().ToLower() == Str.ToLower();
    }
}
