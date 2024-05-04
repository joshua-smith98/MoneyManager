using System.Text;

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
                // Begin with the path to this command, plus the Str
                StringBuilder ret = new();
                ret.Append($"{_pathToThisCommand} {Str}");

                // Case: no arguments -> Do nothing
                if (Arguments.Length == 0) ;

                // Case: One argument with null str -> Only include ID
                else if (Arguments.First().Str is null)
                {
                    var firstArg = Arguments.First();
                    ret.Append(firstArg.IsRequired ? $" [{firstArg.ID}]" : $" ({firstArg.ID})"); // Use square brackets if argument is required, or rounded if not
                }

                // Case: multiple arguments -> include Str and ID
                else
                {
                    for(int i = 0; i < Arguments.Length; i++)
                    {
                        ret.Append($" {Arguments[i].Str}");
                        ret.Append(Arguments.IsRequired ? $" [{Arguments[i].ID}]" : $" ({Arguments[i].ID})"); // Use square brackets if argument is required, or rounded if not
                        ret.Append(i == Arguments.Length - 1 ? ";" : ","); // Put semicolon at end of arg segment if its the last one, otherwise comma
                    }
                }

                // Return built CommandPath
                return ret.ToString();
            }
        }

        public abstract Command[] SubCommands { get; }
        public abstract Argument[] Arguments { get; }

        public abstract Action<ArgumentValueCollection>? Action { get; }

        private string _pathToThisCommand;

        public Command(string pathToThisCommand)
        {
            // Validate constraints
            // Commands with no sub-commands must contain an action
            if (SubCommands.Length == 0 && Action is null)
                throw new REPLSemanticErrorException($"Command path stub (missing action): {CommandPath}"); // Should only be thrown if I make a mistake implementing the command structure
            
            _pathToThisCommand = pathToThisCommand;
        }

        public bool MatchStr(string commandSubStr)
            => commandSubStr.Split().First().ToLower() == Str.ToLower();
    }
}
