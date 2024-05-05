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
