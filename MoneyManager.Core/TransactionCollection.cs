using System.Collections;

namespace MoneyManager.Core
{
    /// <summary>
    /// Represents a collection of <see cref="Transaction"/>s, with an initial and current balance.
    /// </summary>
    /// <param name="initialBalance"></param>
    public class TransactionCollection(MoneyValue initialBalance) : ICollection<Transaction>
    {
        private readonly MoneyValue initialBalance = initialBalance;

        private readonly List<Transaction> transactions = [];

        /// <summary>
        /// Gets the current balance of this collection.
        /// </summary>
        public MoneyValue Balance => BalanceAt(transactions.Last());

        /// <summary>
        /// Gets the number of <see cref="Transaction"/>s contained within this collection.
        /// </summary>
        public int Count => transactions.Count;

        public bool IsReadOnly => false;

        public Transaction this[int index] => transactions[index];

        /// <summary>
        /// Gets the balance of this collection at the given <see cref="Transaction"/>.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public MoneyValue BalanceAt(Transaction transaction)
        {
            // Validity check: transaction must be in this collection
            if (!Contains(transaction)) throw new IndexOutOfRangeException();

            return BalanceAtIndex(transactions.IndexOf(transaction));
        }

        /// <summary>
        /// Gets the balance of this collection at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MoneyValue BalanceAtIndex(int index)
        {
            // Validity check: index must be within the bounds of this collection
            if (index >= transactions.Count) throw new IndexOutOfRangeException();

            MoneyValue total = initialBalance;

            for (int i = 0; i <= index; i++)
            {
                if (transactions[i].IsCleared) total += transactions[i].Value;
            }

            return total;
        }

        public void Add(Transaction item) => transactions.Add(item);

        /// <summary>
        /// Adds a range of <see cref="Transaction"/>s to this collection.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<Transaction> items) => transactions.AddRange(items);

        public void Clear() => transactions.Clear();

        public bool Contains(Transaction item) => transactions.Contains(item);

        public void CopyTo(Transaction[] array, int arrayIndex) => transactions.CopyTo(array, arrayIndex);

        public IEnumerator<Transaction> GetEnumerator() => transactions.GetEnumerator();

        public bool Remove(Transaction item) => transactions.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => transactions.GetEnumerator();
    }
}
