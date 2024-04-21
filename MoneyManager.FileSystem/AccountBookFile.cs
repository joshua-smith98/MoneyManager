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

        internal record CategoryTableRow(string Name, BudgetDat? IncomeBudget, BudgetDat? ExpensesBudget)
        {
            public bool HasIncomeBudget => IncomeBudget is not null;
            public bool HasExpensesBudget => ExpensesBudget is not null;
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
            // Check that file exists
            if (!File.Exists(path)) throw new FileNotFoundException();

            // Open File
            using var br = new BinaryReader(File.OpenRead(path));

            // Validate Structure
            #region Validate
            {
                // Check header
                if (br.ReadChars(8) != Header) throw new InvalidFileFormatException("Tried to load AccountBookFile with invalid header.");

                // Check version and throw if old
                var srcVersion = br.ReadInt32();
                if (srcVersion != Version) throw new OldFileVersionException(srcVersion);

                // Check tables
                try
                {
                    // CategoryTable
                    var numCategories = br.ReadInt32();
                    for (int i = 0; i < numCategories; i++)
                    {
                        br.ReadString(); // Name

                        // IncomeBudget
                        if (br.ReadBoolean())
                        {
                            br.ReadDecimal(); // Value
                            br.ReadInt32(); // Period
                        }

                        // ExpensesBudget
                        if (br.ReadBoolean())
                        {
                            br.ReadDecimal(); // Value
                            br.ReadInt32(); // Period
                        }
                    }

                    // AccountTable
                    var numAccounts = br.ReadInt32();
                    for(int i = 0; i < numAccounts; i++)
                    {
                        br.ReadString(); // Name

                        // TransactionTable
                        var numTransactions = br.ReadInt32();
                        for (int j = 0; j < numTransactions; j++)
                        {
                            br.ReadString(); // TransactionNumber
                            br.ReadDecimal(); // Value
                            br.ReadInt32(); // DayNumber
                            br.ReadString(); // Payee
                            br.ReadString(); // Memo
                            br.ReadString(); // CategoryName
                            br.ReadBoolean(); // IsCleared
                        }
                    }

                    // TransferTable
                    var numTransfers = br.ReadInt32();
                    for(int i = 0; i < numTransfers; i++)
                    {
                        br.ReadString(); // FromAccName
                        br.ReadString(); // ToAccName
                        br.ReadString(); // TransactionNumber
                        br.ReadDecimal(); // Value
                        br.ReadInt32(); // DayNumber
                        br.ReadString(); // Payee
                        br.ReadString(); // Memo
                        br.ReadString(); // CategoryName
                        br.ReadBoolean(); // IsCleared
                    }
                }
                catch (EndOfStreamException)
                {
                    // File is too short
                    throw new InvalidFileFormatException("Tried to load AccountBookFile with invalid format.");
                }
                
                // File is too long
                if (br.BaseStream.Position != br.BaseStream.Length)
                    throw new InvalidFileFormatException("Tried to load AccountBookFile with invalid format.");
            }
            #endregion

            br.BaseStream.Position = 12; // 8 byte header + 4 byte version number

            // Load members from file, construct and return
            #region Load
            {
                // Load CategoryTable
                int numCategories = br.ReadInt32();
                List<CategoryTableRow> categoryTable = [];
                for (int i = 0; i < numCategories; i++)
                {
                    string name = br.ReadString(); // Name

                    // Load IncomeBudget if it exists
                    BudgetDat? incomeBudget = null;
                    if (br.ReadBoolean())
                    {
                        incomeBudget = new BudgetDat(
                            br.ReadDecimal(), // Value
                            br.ReadInt32() // Period
                            );
                    }

                    // Load ExpensesBudget if it exists
                    BudgetDat? expensesBudget = null;
                    if (br.ReadBoolean())
                    {
                        expensesBudget = new BudgetDat(
                            br.ReadDecimal(), // Value
                            br.ReadInt32() // Period
                            );
                    }

                    // Construct CategoryTableRow and add to list
                    categoryTable.Add(new CategoryTableRow(name, incomeBudget, expensesBudget));
                }

                // Load AccountTable
                int numAccounts = br.ReadInt32();
                List<AccountTableRow> accountTable = [];
                for (int i = 0; i < numAccounts; i++)
                {
                    string name = br.ReadString(); // Name

                    // Load TransactionTable
                    int numTransactions = br.ReadInt32();
                    List<TransactionTableRow> transactionTable = [];
                    for (int j = 0; j < numTransactions; j++)
                    {
                        // Construct TransactionTableRow and add to table all in one go
                        transactionTable.Add(
                            new TransactionTableRow(
                                br.ReadString(), // TransactionNumber
                                br.ReadDecimal(), // Value
                                br.ReadInt32(), // DayNumber
                                br.ReadString(), // Payee
                                br.ReadString(), // Memo
                                br.ReadString(), // CategoryName
                                br.ReadBoolean() // IsCleared
                                )
                            );

                    }

                    // Add to AccountTable
                    accountTable.Add(new AccountTableRow(name, numTransactions, transactionTable.ToArray()));
                }

                // Load TransferTable
                int numTransfers = br.ReadInt32();
                List<TransferTableRow> transferTable = [];
                for (int i = 0; i < numTransfers; i++)
                {
                    // Construct TransferTableRow and add to TransferTable all in one go
                    transferTable.Add(
                        new TransferTableRow(
                            br.ReadString(), // FromAccName
                            br.ReadString(), // ToAccName
                            br.ReadString(), // TransactionNumber
                            br.ReadDecimal(), // Value
                            br.ReadInt32(), // DayNumber
                            br.ReadString(), // Payee
                            br.ReadString(), // Memo
                            br.ReadString(), // CategoryName
                            br.ReadBoolean() // IsCleared
                            )
                        );
                }

                // Construct AccountBookFile and return
                return new AccountBookFile(
                    numCategories,
                    categoryTable.ToArray(),
                    numAccounts,
                    accountTable.ToArray(),
                    numTransfers,
                    transferTable.ToArray(),
                    path
                    );
            }
            #endregion
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
