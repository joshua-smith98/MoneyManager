using System.Collections;
using System.Diagnostics;

namespace MoneyManager.Core
{
    public class TransactionCollection(MoneyValue initialBalance) : ICollection<Transaction>
    {
        private readonly MoneyValue initialBalance = initialBalance;

        private readonly List<Transaction> transactions = [];

        public MoneyValue Balance => BalanceAt(transactions.Last());
        
        public int Count => transactions.Count;

        public bool IsReadOnly => false;

        public Transaction this[int index] => transactions[index];

        public MoneyValue BalanceAt(Transaction transaction)
        {
            if (!Contains(transaction)) throw new IndexOutOfRangeException();

            return BalanceAtIndex(transactions.IndexOf(transaction));
        }

        public MoneyValue BalanceAtIndex(int index)
        {
            MoneyValue total = initialBalance;

            for (int i = 0; i <= index; i++)
            {
                if (transactions[i].IsCleared) total += transactions[i].Value;
            }

            return total;
        }

        public void Add(Transaction item) => transactions.Add(item);

        public void AddRange(IEnumerable<Transaction> items) => transactions.AddRange(items);

        public void Clear() => transactions.Clear();

        public bool Contains(Transaction item) => transactions.Contains(item);

        public void CopyTo(Transaction[] array, int arrayIndex) => transactions.CopyTo(array, arrayIndex);

        public IEnumerator<Transaction> GetEnumerator() => transactions.GetEnumerator();

        public bool Remove(Transaction item) => transactions.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => transactions.GetEnumerator();
    }
}
