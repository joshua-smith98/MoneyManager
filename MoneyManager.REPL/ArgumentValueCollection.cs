using System.Collections;

namespace MoneyManager.REPL
{
    internal class ArgumentValueCollection : IEnumerable<KeyValuePair<string, ArgumentValue>>
    {
        private readonly Dictionary<string, ArgumentValue> keyValuePairs = [];
        
        public ArgumentValue this[string ID] => keyValuePairs[ID];

        public IEnumerable<string> IDs => keyValuePairs.Keys;

        public IEnumerable<ArgumentValue> Values => keyValuePairs.Values;

        public int Count => keyValuePairs.Count;

        public void Add(string ID, ArgumentValue Value)
            => keyValuePairs.Add(ID, Value);

        public void Add(ArgumentValueCollection argumentValues)
        {
            foreach (KeyValuePair<string, ArgumentValue> keyValuePair in argumentValues)
                keyValuePairs.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsID(string ID)
            => keyValuePairs.ContainsKey(ID);

        public IEnumerator<KeyValuePair<string, ArgumentValue>> GetEnumerator()
            => keyValuePairs.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
