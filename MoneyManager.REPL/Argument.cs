using System.Reflection.Metadata.Ecma335;

namespace MoneyManager.REPL
{
    internal abstract class Argument(string id, bool isRequired, string? str = null)
    {
        public string? Str { get; } = str;
        public string ID { get; } = id;

        [Obsolete]
        public bool IsRequired { get; } = isRequired; // TODO: Properly remove this property and re-factor Command.CommandPath to use Command.RequiredArgIDs

        public abstract Type Type { get; }

        public bool MatchStr(string argumentSubStr)
            => Str is null || Str is not null && argumentSubStr.Split().First().ToLower() == Str.ToLower();

        public ArgumentValue TryRead(string argumentSubStr)
        {
            // If we're expecting Str, then everything except that is our argument
            if (Str is not null)
                return Parse(argumentSubStr.Trim()[Str.Length..].Trim());

            // Otherwise, if we aren't expecting Str, then the whole substr must be our argument
            else
                return Parse(argumentSubStr);
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
