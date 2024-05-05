namespace MoneyManager.REPL
{
    internal abstract class Argument(string id, bool isRequired, string? str = null)
    {
        public string? Str { get; } = str;
        public string ID { get; } = id;

        [Obsolete]
        public bool IsRequired { get; } = isRequired; // TODO: Properly remove this property and re-factor Command.CommandPath to use Command.RequiredArgIDs

        public abstract Type Type { get; }

        public ArgumentValue? TryRead(string argumentSubStr)
        {
            var argWords = argumentSubStr.Split();

            // If we're expecting Str and it matches the first word, then everything except that is our argument
            if (Str is not null && argWords.First().ToLower() == Str.ToLower())
                return Parse(argumentSubStr[argWords.First().Length..].Trim());

            // Otherwise, if we aren't expecting Str, then the whole substr must be our argument
            else if (Str is null)
                return Parse(argumentSubStr);

            // Finally, if we're expecting string, but it doesn't match then this isn't the argument we're looking for
            else return null;
        }

        protected abstract ArgumentValue Parse(string argumentSubStr);
    }
}
