namespace MoneyManager.Core
{
    public readonly struct BalanceInfo
    {
        public readonly Money Balance;
        public readonly Money ClearedBalance;

        public readonly Money Income;
        public readonly Money ClearedIncome;

        public readonly Money Expenses;
        public readonly Money ClearedExpenses;

        public BalanceInfo(Transaction[] transactions)
        {
            // Calculate balances
            Balance = transactions
                .Select(x => x.Value)
                .Aggregate((total, next) => total + next);
            ClearedBalance = transactions
                .Select(x => x.ClearedValue)
                .Aggregate((total, next) => total + next);

            // Calculate incomes
            var incomes = transactions
                .Where(x => x.TransactionType is TransactionType.Deposit);
            Income = incomes
                .Select(x => x.Value)
                .Aggregate((total, next) => total + next);
            ClearedIncome = incomes
                .Select(x => x.ClearedValue)
                .Aggregate((total, next) => total + next);

            // Calculate expenses
            var expenses = transactions
                .Where(x => x.TransactionType is TransactionType.Withdrawal);
            Expenses = transactions
                .Select(x => x.Value)
                .Aggregate((total, next) => total + next);
            ClearedExpenses = transactions
                .Select(x => x.ClearedValue)
                .Aggregate((total, next) => total + next);
        }
    }
}
