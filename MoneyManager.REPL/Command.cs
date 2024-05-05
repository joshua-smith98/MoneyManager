using System.Text;

namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents a REPL command (i.e. some word) within a command path. Optionally contains some sub-commands, arguments and/or an <see cref="System.Action"/>.<br/>
    /// If a <see cref="Command"/> has no sub-commands, then it must contain an Action.
    /// </summary>
    internal abstract class Command
    {
        /// <summary>
        /// The string the user uses to refer to this command within a command path. This should be lowercase.
        /// </summary>
        public abstract string Str { get; }

        /// <summary>
        /// A general description of this command. Used for help information.
        /// </summary>
        public abstract string About { get; }
        /// <summary>
        /// Gets the command path for this command, including all required and/or optional arguments.
        /// </summary>
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

        /// <summary>
        /// The collection of sub-commands for this command. If this is empty, then <see cref="Action"/> must not be null.
        /// </summary>
        public abstract Command[] SubCommands { get; }
        /// <summary>
        /// The collection of <see cref="Argument"/>s associated with this command.
        /// </summary>
        public abstract Argument[] Arguments { get; }
        /// <summary>
        /// The list of <see cref="Argument"/>s that are required by this command, referenced by their <see cref="Argument.ID"/>s.
        /// </summary>
        public abstract string[] RequiredArgIDs { get; }

        /// <summary>
        /// The <see cref="System.Action"/> associated with this command. If this is null, then this command must contain some sub-commands.
        /// </summary>
        public abstract Action<ArgumentValueCollection>? Action { get; }

        private string _pathToThisCommand;

        /// <summary>
        /// Constructs a new instance of <see cref="Command"/>.
        /// </summary>
        /// <param name="pathToThisCommand"></param>
        /// <exception cref="REPLSemanticErrorException"></exception>
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

        /// <summary>
        /// Returns <see cref="true"/> if this command's <see cref="Str"/> matches <paramref name="commandSubStr"/>.
        /// </summary>
        /// <param name="commandSubStr"></param>
        /// <returns></returns>
        public bool MatchStr(string commandSubStr)
            => commandSubStr.Split().First().ToLower() == Str.ToLower();

        /// <summary>
        /// Parses the given string and runs this command, using the given arguments aquired further back in the command path.<br/>
        /// Assumes that <see cref="MatchStr"/> returns <see cref="true"/> for <paramref name="commandSubStr"/>.
        /// </summary>
        /// <param name="commandSubStr">A substring of the user input, beginning at this command's <see cref="Str"/></param>
        /// <param name="previousArgs"></param>
        /// <exception cref="REPLSemanticErrorException"></exception>
        /// <exception cref="REPLCommandPathNotValidException"></exception>
        /// <exception cref="REPLCommandNotFoundException"></exception>
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

        /// <summary>
        /// Reads the arguments for this command from the given string.
        /// </summary>
        /// <param name="argsSubStr">A substring of the user input, beginning at the arguments for this command.</param>
        /// <returns></returns>
        /// <exception cref="REPLArgumentNotFoundException"></exception>
        /// <exception cref="REPLSemanticErrorException"></exception>
        private ArgumentValueCollection ReadArgs(string argsSubStr)
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
