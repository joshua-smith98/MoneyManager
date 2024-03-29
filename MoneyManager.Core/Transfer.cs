namespace MoneyManager.Core
{
    internal class Transfer : Transaction
    {
        public Account To { get; }
        public Account From { get; }
        public new TransactionType TransactionType => TransactionType.Transfer;

        public bool TransfersTo(Account account) => ReferenceEquals(account, To);
        public bool TransfersFrom(Account account) => ReferenceEquals(account, From);

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
    }
}
