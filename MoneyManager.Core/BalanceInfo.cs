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
            // Check for empty transactions[]
            if (!transactions.Any())
            {
                Balance = 0;
                ClearedBalance = 0;
                Income = 0;
                ClearedIncome = 0;
                Expenses = 0;
                ClearedExpenses = 0;
                return;
            }
            
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
            if (incomes.Any()) // Case: some income exists
            {
                Income = incomes
                    .Select(x => x.Value)
                    .Aggregate((total, next) => total + next);
                ClearedIncome = incomes
                    .Select(x => x.ClearedValue)
                    .Aggregate((total, next) => total + next);
            }
            else // Case: no income (is poor) (an exception is thrown if we don't check for this)
            {
                Income = 0;
                ClearedIncome = 0;
            }

            // Calculate expenses
            var expenses = transactions
                .Where(x => x.TransactionType is TransactionType.Withdrawal);
            if (expenses.Any()) // Case: expenses exist
            {
                Expenses = expenses
                    .Select(x => x.Value)
                    .Aggregate((total, next) => total + next);
                ClearedExpenses = expenses
                    .Select(x => x.ClearedValue)
                    .Aggregate((total, next) => total + next);
            }
            else // Case: no expenses
            {
                Expenses = 0;
                ClearedExpenses = 0;
            }
        }
    }
}
