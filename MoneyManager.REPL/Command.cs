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

        public void Parse(string commandSubStr, ArgumentValueCollection previousArgs)
        {
            // Assume MatchStr has already been determined to be true

            // Remove Str from commandSubStr
            var remainder = commandSubStr.Trim()[Str.Length..].Trim();

            // Try and match remainder to subcommand first, if so then Parse that and return
            var subCommandMatches = SubCommands.Where(x => x.MatchStr(remainder));

            if (subCommandMatches.Count() > 1)
                throw new REPLSemanticErrorException($"Multiple subcommands in command \"{Str}\" match str: \"{remainder.Split().First()}\"");

            if (subCommandMatches.Count() == 1)
            {
                subCommandMatches.First().Parse(commandSubStr, previousArgs);
                return;
            }

            // Then try parsing remainder as arguments - split until semicolon excluding quotes
            var argsSubStr = Argument.SplitOutside(remainder, ';', '"').First();
            previousArgs.Add(ReadArgs(argsSubStr));

            // Finally, if nothing remains then run our action; otherwise parse remainder as subcommand and return
            var finalRemainder = remainder[argsSubStr.Length..].Trim();
            
            if (finalRemainder == string.Empty)
            {
                if (Action is null)
                    throw new REPLCommandPathNotValidException($"Found nothing after command \"{Str}\" or its arguments. This command isn't at the end of a valid command path!");
                
                Action.Invoke(previousArgs);
                return;
            }
            else
            {
                subCommandMatches = SubCommands.Where(x => x.MatchStr(finalRemainder));

                if (subCommandMatches.Count() > 1)
                    throw new REPLSemanticErrorException($"Multiple subcommands in command \"{Str}\" match str: \"{finalRemainder.Split().First()}\"");

                if (subCommandMatches.Count() == 1)
                {
                    subCommandMatches.First().Parse(commandSubStr, previousArgs);
                    return;
                }
            }

            // If parsing of final remainder fails then we couldn't find the next command\
            throw new REPLCommandNotFoundException($"Command \"{Str}\" doesn't contain any sub-command: \"{finalRemainder.Split().First()}\"");
        }

        public ArgumentValueCollection ReadArgs(string argsSubStr)
        {
            // Assume that this is a substring goes from the beginning of the args to the end including semi-colon, and doesn't include the command str
            // Read args in any order and none are read twice
            var ret = new ArgumentValueCollection();

            // Case: No arguments -> do nothing, return empty collection
            if (Arguments.Length == 0) ;

            // Case: One argument with null Str -> Assume argsSubStr is the whole single argument
            else if (Arguments.Length == 1 && Arguments.First().Str is null)
                ret.Add(Arguments.First().ID, Arguments.First().Parse(argsSubStr)!); // TryRead will always return non-null here, as we're not checking str

            // Case: One or more arguments w/ Str
            else
            {
                // Split argSubStrs by comma, ignoring commas inside double-quotes
                var argSubStrs = Argument.SplitOutside(argsSubStr, ',', '"');

                // Read any arguments that match each argSubStr and remove them from the match pool if they do (so we only match them once)
                var argList = Arguments.ToList();
                foreach (var argSubStr in argSubStrs)
                {
                    var matchResults = argList.Where(x => x.MatchStr(argSubStr));

                    // Case: no arguments match
                    if (!matchResults.Any())
                        throw new REPLArgumentNotFoundException($"Couldn't find argument for command \"{Str}\" with label: \"{argSubStr.Split().First()}\"");

                    // Case: more than one arguments match -> Must mean identical IDs within a single command, this is problem with my code!
                    if (matchResults.Count() > 1)
                        throw new REPLSemanticErrorException($"Multiple arguments in command \"{Str}\" match Str: \"{argsSubStr.Split().First()}\"");

                    ret.Add(matchResults.First().ID, matchResults.First().Parse(argsSubStr));
                    argList.Remove(matchResults.First());
                }
            }

            // Return
            return ret;
        }
    }
}
