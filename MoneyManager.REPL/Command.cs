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
                        ret.Append(Arguments[i].IsRequired ? $" [{Arguments[i].ID}]" : $" ({Arguments[i].ID})"); // Use square brackets if argument is required, or rounded if not
                        ret.Append(i == Arguments.Length - 1 ? ";" : ","); // Put semicolon at end of arg segment if its the last one, otherwise comma
                    }
                }

                // Return built CommandPath
                return ret.ToString();
            }
        }

        public abstract Command[] SubCommands { get; }
        public abstract Argument[] Arguments { get; }
        public abstract string[] RequiredArgIDs { get; }

        public abstract Action<ArgumentValueCollection>? Action { get; }

        private string _pathToThisCommand;

        public Command(string pathToThisCommand = "")
        {
            // Validate constraints
            // Commands with no sub-commands must contain an action
            if (SubCommands.Length == 0 && Action is null)
                throw new REPLSemanticErrorException($"Command path stub (missing action): {CommandPath}"); // Should only be thrown if I make a mistake implementing the command structure

            // Only lonely arguments may contain no Str
            if (Arguments.Length > 1 && Arguments.Any(x => x.Str is null))
                throw new REPLSemanticErrorException($"One or more arguments missing Str when more than one exist: {CommandPath}"); // Ditto^^

            _pathToThisCommand = pathToThisCommand;
        }

        public bool MatchStr(string commandSubStr)
            => commandSubStr.Split().First().ToLower() == Str.ToLower();

        public ArgumentValueCollection ReadArgs(string argsSubStr)
        {
            // Whooh this method is a doozy...
            // Assume that this is a substring goes from the beginning of the args to the end including semi-colon, and doesn't include the command str
            // Read args in any order and none are read twice
            var ret = new ArgumentValueCollection();

            // Case: No arguments -> do nothing, return empty collection
            if (Arguments.Length == 0) ;

            // Case: One argument with null Str -> Assume argsSubStr is the whole single argument
            else if (Arguments.Length == 1 && Arguments.First().Str is null)
                ret.Add(Arguments.First().ID, Arguments.First().TryRead(argsSubStr)!); // TryRead will always return non-null here, as we're not checking str

            // Case: One or more arguments w/ Str
            else
            {
                // Split argSubStr into separate arguments via comma
                var commaSplit = argsSubStr.Split(',');

                // Then concatinate any strings between quotes
                var argSubStrList = new List<string>();
                var insideQuotedSegment = false;
                for (int i = 0; i < commaSplit.Length; i++)
                {
                    if (!insideQuotedSegment)
                        argSubStrList.Add(commaSplit[i]);
                    else
                        argSubStrList[^1] += commaSplit[i];

                    // If we detect a single quote only within a segment, we move inside or outside a quoted segment
                    if (commaSplit[i].Where(x => x == '"').Count() == 1)
                        insideQuotedSegment = !insideQuotedSegment;
                }

                // Read any arguments that match each argSubStr once only
                var argList = Arguments.ToList();
                foreach (var argSubStr in argSubStrList)
                {
                    foreach (var argument in argList)
                    {
                        var tryReadResult = argument.TryRead(argSubStr);
                        if (tryReadResult is not null)
                        {
                            ret.Add(argument.ID, tryReadResult);
                            argList.Remove(argument);
                            break;
                        }
                    }
                }
            }

            // Return
            return ret;
        }
    }
}
