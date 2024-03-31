namespace MoneyManager.Core
{
    public abstract class Balanceable
    {
        public abstract Transaction[] Transactions { get; }
        public virtual BalanceInfo BalanceInfo => new BalanceInfo(Transactions);

        public virtual BalanceInfo BalanceInfoAt(Transaction transaction)
        {
            // Validity check: transaction must be in Transactions
            if (!Transactions.Contains(transaction)) throw new IndexOutOfRangeException();

            return BalanceInfoAtIndex(Transactions.ToList().IndexOf(transaction));
        }

        public virtual BalanceInfo BalanceInfoAtIndex(int index)
        {
            // Validity check: index must be within bounds of Transactions[]
            if (index >= Transactions.Length) throw new IndexOutOfRangeException();

            return new BalanceInfo(Transactions[..(index + 1)]); // End of range is exclusive, but our 'index' is inclusive
        }

        public virtual BalanceInfo BalanceInfoBetween(Transaction from, Transaction to)
        {
            // Validity check: from and to must be within Transactions[]
            if (!(Transactions.Contains(from) && Transactions.Contains(to))) throw new IndexOutOfRangeException();

            var transactionList = Transactions.ToList();
            return BalanceInfoBetweenIndices(transactionList.IndexOf(from), transactionList.IndexOf(to));
        }

        public virtual BalanceInfo BalanceInfoBetweenIndices(int from, int to)
        {
            // Validity check, from must be less then to
            if (from >= to) throw new ArgumentOutOfRangeException(nameof(from), "Argument 'from' must be less than argument 'to'.");

            return new BalanceInfo(Transactions[from..(to + 1)]); // End of range is exclusive, but our 'to' is inclusive
        }
    }
}
