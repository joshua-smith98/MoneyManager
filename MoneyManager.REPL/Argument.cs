namespace MoneyManager.REPL
{
    /// <summary>
    /// Represents an Argument in the MoneyManager REPL, connected with some <see cref="Command"/>.
    /// </summary>
    /// <param name="id">The internal identifier for this argument.</param>
    /// <param name="isRequired">Determines whether this argument is required by the command it is contained within.</param>
    /// <param name="str">The string used by the user to refer to this argument when specifying a value.</param>
    internal abstract class Argument(string id, bool? isRequired = null, string? str = null)
    {
        /// <summary>
        /// The string used by the user to refer to this argument instance when specifying a value.<br/>
        /// If this is <see cref="null"/>, then this must be the only <see cref="Argument"/> connected with its <see cref="Command"/>.
        /// </summary>
        public string? Str { get; } = str;
        /// <summary>
        /// The internal identifier for this argument instance. Usually a more descriptive variant of <see cref="Str"/>.<br/>
        /// Note that no two identifiers can be the same for any arguments within a command path.
        /// </summary>
        public string ID { get; } = id;

        /// <summary>
        /// Determines whether this argument is required by the command it is contained within.<br/>
        /// Note: This does not set a requirement for commands further down in the path, and should not be used within an unactionable command.
        /// </summary>
        public bool? IsRequired { get; } = isRequired;

        /// <summary>
        /// The value type that this argument accepts.
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// Returns true if this argument's <see cref="Str"/> matches the given string.
        /// </summary>
        /// <param name="argumentSubStr">A substring of the user input, beginning from this argument's <see cref="Str"/>, or a value if <see cref="Str"/> is null.</param>
        /// <returns></returns>
        public bool MatchStr(string argumentSubStr)
            => Str is null || Str is not null && argumentSubStr.Split().First().ToLower() == Str.ToLower();

        /// <summary>
        /// Parses the given string to get a value. Assumes that <see cref="MatchStr"/> returns true for <paramref name="argumentSubStr"/>.
        /// </summary>
        /// <param name="argumentSubStr">A substring of the user input, beginning from this argument's <see cref="Str"/>, or a value if <see cref="Str"/> is null.</param>
        /// <returns></returns>
        public ArgumentValue Parse(string argumentSubStr)
        {
            // If we're expecting Str, then everything except that is our argument
            if (Str is not null)
                return ParseImpl(argumentSubStr.Trim()[Str.Length..].Trim());

            // Otherwise, if we aren't expecting Str, then the whole substr must be our argument
            else
                return ParseImpl(argumentSubStr);
        }

        /// <summary>
        /// Local implementation to parse <paramref name="argumentSubStr"/> for some typed value.
        /// </summary>
        /// <param name="argumentSubStr"></param>
        /// <exception cref="REPLArgumentParseException"/>
        /// <returns></returns>
        protected abstract ArgumentValue ParseImpl(string argumentSubStr);

        /// <summary>
        /// Performs the same function as <see cref="string.Split(char[]?)"/>, but ignores all separators between instances of <paramref name="ignoreSplitInside"/>.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <param name="ignoreSplitInside"></param>
        /// <returns></returns>
        internal static string[] SplitOutside(string input, char separator, char ignoreSplitInside)
        {
            // Split input into separate arguments via separator
            var commaSplit = input.Split(separator);

            // Then concatinate any strings between ignore char
            var outputList = new List<string>();
            var insideIgnoredegment = false;
            for (int i = 0; i < commaSplit.Length; i++)
            {
                if (!insideIgnoredegment)
                    outputList.Add(commaSplit[i]);
                else
                    outputList[^1] += commaSplit[i];

                // If we detect an odd number of ignore char only within a segment, we move inside or outside an ignored segment
                if (commaSplit[i].Where(x => x == ignoreSplitInside).Count() % 2 != 0)
                    insideIgnoredegment = !insideIgnoredegment;
            }

            return outputList.ToArray();
        }
    }
}
