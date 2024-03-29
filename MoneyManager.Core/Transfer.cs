namespace MoneyManager.Core
{
    /// <summary>
    /// A special <see cref="Transaction"/> that represents a transfer of money between two <see cref="Account"/>s.
    /// </summary>
    internal class Transfer : Transaction
    {
        /// <summary>
        /// The <see cref="Account"/> the money was transfered to.
        /// </summary>
        public Account To { get; }

        /// <summary>
        /// The <see cref="Account"/> the money was transferred from.
        /// </summary>
        public Account From { get; }

        public override TransactionType TransactionType => TransactionType.Transfer;

        public Transfer(Account to, Account from, MoneyValue value, string payee, DateOnly date, string memo, string number)
            : base(value, payee, date, memo, number)
        {
            To = to;
            From = from;
        }

        public Transfer(Account to, Account from, MoneyValue value, string payee, DateOnly date, string memo)
            : base(value, payee, date, memo)
        {
            To = to;
            From = from;
        }

        public Transfer(Account to, Account from, MoneyValue value, string payee, DateOnly date)
            : base(value, payee, date)
        {
            To = to;
            From = from;
        }

        public Transfer(Account to, Account from, MoneyValue value, string payee, string memo)
            : base(value, payee, memo)
        {
            To = to;
            From = from;
        }

        public Transfer(Account to, Account from, MoneyValue value, string payee)
            : base(value, payee)
        {
            To = to;
            From = from;
        }

        /// <summary>
        /// Returns <see cref="true"/> if the given <see cref="Account"/> is the one money was transferred to.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool TransfersTo(Account account) => ReferenceEquals(account, To);

        /// <summary>
        /// Returns <see cref="true"/> if the given <see cref="Account"/> is the one money was transferred from.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool TransfersFrom(Account account) => ReferenceEquals(account, From);
    }
}
