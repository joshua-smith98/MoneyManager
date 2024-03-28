namespace MoneyManager.Core
{
    public class Transaction
    {
        public string? Number { get; set; }
        public MoneyValue Value { get; set; }
        public DateOnly Date { get; set; }
        public string Payee { get; set; }
        public string Memo { get; set; }
        //public BudgetCategory? Category { get; } // BudgetCategory yet to be implemented
        public bool IsCleared { get; set; } = true;

        public TransactionType TransactionType
        {
            get
            {
                if (Value > 0) return TransactionType.Deposit;
                if (Value < 0) return TransactionType.Withdrawal;
                return TransactionType.Null;
            }
        }

        public Transaction(string number, MoneyValue value, DateOnly date, string payee, string memo)
        {
            Number = number;
            Value = value;
            Date = date;
            Payee = payee;
            Memo = memo;
        }

        public Transaction(MoneyValue value, DateOnly date, string payee, string memo)
        {
            Value = value;
            Date = date;
            Payee = payee;
            Memo = memo;
        }
    }
}
