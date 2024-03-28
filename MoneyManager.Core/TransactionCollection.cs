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

        public MoneyValue BalanceAt(Transaction transaction)
        {
            if (!Contains(transaction)) throw new IndexOutOfRangeException();

            var limit = transactions.IndexOf(transaction);
            MoneyValue total = initialBalance;

            for (int i = 0; i <= limit; i++)
            {
                if (transactions[i].IsCleared) total += transactions[i].Value;
            }

            return total;
        }

        public void Add(Transaction item) => transactions.Add(item);

        public void Clear() => transactions.Clear();

        public bool Contains(Transaction item) => transactions.Contains(item);

        public void CopyTo(Transaction[] array, int arrayIndex) => transactions.CopyTo(array, arrayIndex);

        public IEnumerator<Transaction> GetEnumerator() => transactions.GetEnumerator();

        public bool Remove(Transaction item) => transactions.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => transactions.GetEnumerator();
    }
}
