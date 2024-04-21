using MoneyManager.Core;

namespace MoneyManager.FileSystem
{
    public class AccountBookFile : IFile<AccountBook, AccountBookFile>
    {
        public static char[] Header => "MOMAACBK".ToArray();

        public static string Extension => ".accbk";

        public static int Version => 1;

        public string? Path { get; private set; }

        internal int NumCategories;
        internal CategoryTableRow[] CategoryTable;
        internal int NumAccounts;
        internal AccountTableRow[] AccountTable;
        internal int NumTransfers;
        internal TransferTableRow[] TransferTable;

        internal record CategoryTableRow(string Name)
        {
            public BudgetDat? IncomeBudget { get; init; }
            public BudgetDat? ExpensesBudget { get; init; }
        }

        internal record BudgetDat(decimal PerDay, int Period);

        internal record AccountTableRow(
            string Name,
            int NumTransactions,
            TransactionTableRow[] TransactionTable
            );

        internal record TransactionTableRow(
            string TransactionNumber,
            decimal Value,
            int DayNumber,
            string Payee,
            string Memo,
            string CategoryName,
            bool IsCleared
            );

        internal record TransferTableRow(
            string FromAccName,
            string ToAccName,
            string TransactionNumber,
            decimal Value,
            int DayNumber,
            string Payee,
            string Memo,
            string CategoryName,
            bool IsCleared
            );

        private AccountBookFile(
            int numCategories,
            CategoryTableRow[] categoryTable,
            int numAccounts,
            AccountTableRow[] accountTable,
            int numTransfers,
            TransferTableRow[] transferTable,
            string? path = null
            )
        {
            NumCategories = numCategories;
            CategoryTable = categoryTable;
            NumAccounts = numAccounts;
            AccountTable = accountTable;
            NumTransfers = numTransfers;
            TransferTable = transferTable;
            Path = path;
        }

        public static AccountBookFile LoadFrom(string path)
        {
            throw new NotImplementedException();
        }

        public static AccountBookFile Deconstruct(AccountBook accountBook)
        {
            throw new NotImplementedException();
        }

        public void UpdateFrom(AccountBook accountBook)
        {
            throw new NotImplementedException();
        }

        public void SaveTo(string path)
        {
            throw new NotImplementedException();
        }

        public AccountBook Construct()
        {
            throw new NotImplementedException();
        }

    }
}
