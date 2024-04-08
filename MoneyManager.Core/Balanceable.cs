namespace MoneyManager.Core
{
    /// <summary>
    /// Base class for anything that contains <see cref="Transaction"/>s and can be balanced. Inherited by <see cref="Account"/> and <see cref="Category"/>.
    /// </summary>
    public abstract class Balanceable
    {
        public abstract Transaction[] Transactions { get; }
        
        /// <summary>
        /// Gets the Balance, Income and Expenses of this object.
        /// </summary>
        public virtual BalanceInfo BalanceInfo => new BalanceInfo(Transactions);

        /// <summary>
        /// Gets the Balance, Income and Expenses of this object, up until the given <see cref="Transaction"/>, inclusive.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual BalanceInfo BalanceInfoAt(Transaction transaction)
        {
            // Validity check: transaction must be in Transactions
            if (!Transactions.Contains(transaction)) throw new IndexOutOfRangeException();

            return BalanceInfoAtIndex(Transactions.ToList().IndexOf(transaction));
        }

        /// <summary>
        /// Gets the Balance, Income and Expenses of this object, up until the given index, inclusive.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual BalanceInfo BalanceInfoAtIndex(int index)
        {
            // Validity check: index must be within bounds of Transactions[]
            if (index < 0 || index >= Transactions.Length) throw new IndexOutOfRangeException();

            return new BalanceInfo(Transactions[..(index + 1)]); // End of range is exclusive, but our 'index' is inclusive
        }

        public virtual BalanceInfo BalanceInfoAtDate(DateOnly date)
            => new BalanceInfo(Transactions.Where(x => x.Date <= date).ToArray());

        /// <summary>
        /// Gets the Balance, Income and Expenses of this object, between the given <see cref="Transaction"/>s, inclusive.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public virtual BalanceInfo BalanceInfoBetween(Transaction from, Transaction to)
        {
            // Validity check: from and to must be within Transactions[]
            if (!(Transactions.Contains(from) && Transactions.Contains(to))) throw new IndexOutOfRangeException();

            var transactionList = Transactions.ToList();
            return BalanceInfoBetweenIndices(transactionList.IndexOf(from), transactionList.IndexOf(to));
        }

        /// <summary>
        /// Gets the Balance, Income and Expenses of this object, between the given indices, inclusive.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual BalanceInfo BalanceInfoBetweenIndices(int from, int to)
        {   
            // Validity check, from must be less then to
            if (from >= to) throw new ArgumentOutOfRangeException(nameof(from), "Argument 'from' must be less than argument 'to'.");

            // Validity check: from and to must be within the bounds of Transactions[]
            if (from < 0 || to >= Transactions.Length) throw new IndexOutOfRangeException(); // Since 'from' must be less than 'to', we only need to check 'to' here

            return new BalanceInfo(Transactions[from..(to + 1)]); // End of range is exclusive, but our 'to' is inclusive
        }

        /// <summary>
        /// Gets the <see cref="BalanceInfo"/> for this object's transactions, for the given period starting from the given date.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public virtual BalanceInfo BalanceInfoForPeriod(DateOnly from, Period period)
        {
            // Get the date the period extends to, being careful to be inclusive (which is why the weirdness is there)
            var to = period.GetEndDate(from);

            var transactionsBetween = Transactions.Where(x => x.Date >= from && x.Date <= to);
            return BalanceInfoBetween(transactionsBetween.First(), transactionsBetween.Last());
        }
    }
}
