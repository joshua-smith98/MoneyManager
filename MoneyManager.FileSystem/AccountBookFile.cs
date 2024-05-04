using MoneyManager.Core;

namespace MoneyManager.FileSystem
{
    /// <summary>
    /// Represents the file format that can be used to transmute between some <see cref="AccountBookFile"/> and a file on the disk.
    /// </summary>
    public class AccountBookFile : IFile<AccountBook, AccountBookFile>
    {
        public static char[] Header => "MOMAACBK".ToArray();

        public static string Extension => ".accbk";

        public static int Version => 1;

        public string? Path { get; private set; }

        #region FileDataStructure
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

        internal record BudgetDat(decimal PerPeriod, int Period);

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
        #endregion

        /// <summary>
        /// Loads an instance of <see cref="AccountBookFile"/> from the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="InvalidFileFormatException"></exception>
        /// <exception cref="OldFileVersionException"></exception>
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

        /// <summary>
        /// Deconstructs the given <see cref="AccountBook"/> into an instance of <see cref="AccountBookFile"/>.
        /// </summary>
        /// <param name="accountBook"></param>
        /// <returns></returns>
        public static AccountBookFile Deconstruct(AccountBook accountBook)
        {
            // Construct CategoryTable
            int numCategories = accountBook.Categories.Length;
            var categoryTable = new CategoryTableRow[numCategories];

            for (int i = 0; i < numCategories; i++)
            {
                var category = accountBook.Categories[i];

                string name = category.Name;

                BudgetDat? incomeBudget =
                    category.IncomeBudget is not null
                    ? new BudgetDat((decimal)category.IncomeBudget.Get(), (int)category.IncomeBudget.CurrentPeriod)
                    : null;

                BudgetDat? expensesBudget =
                    category.ExpensesBudget is not null
                    ? new BudgetDat((decimal)category.ExpensesBudget.Get(), (int)category.ExpensesBudget.CurrentPeriod)
                    : null;

                categoryTable[i] = new CategoryTableRow(name, incomeBudget, expensesBudget);
            }

            // Construct AccountTable & Build Transfer List
            int numAccounts = accountBook.Accounts.Length;
            var accountTable = new AccountTableRow[numAccounts];

            List<Transfer> transfers = [];

            for (int i = 0; i < numAccounts; i++)
            {
                var account = accountBook.Accounts[i];
                
                string name = account.Name;

                // Construct TransactionTable
                int numTransactions = account.Transactions.Length;
                List<TransactionTableRow> transactionTable = []; // Use a list here, since we'll only be including transactions, and not transfers

                for (int j = 0; j < numTransactions; j++)
                {
                    var transaction = account.Transactions[j];

                    // Add to Transfer list if it's a transfer
                    if (transaction is Transfer transfer)
                    {
                        if (transfer.Value > 0) transfers.Add(transfer); // Only add transfers with positive values (to avoid doubling up via twins)
                        continue; // Never add transfer to TransactionTable
                    }

                    // Otherwise add row to transactionTable
                    transactionTable.Add(new TransactionTableRow(
                        transaction.Number,
                        (decimal)transaction.Value,
                        transaction.Date.DayNumber,
                        transaction.Payee,
                        transaction.Memo,
                        transaction.Category is not null ? transaction.Category.Name : "", // Use empty category name for null category
                        transaction.IsCleared
                        ));
                }

                // Refresh numTransactions with actual number
                numTransactions = transactionTable.Count;

                // Construct AccountTableRow
                accountTable[i] = new AccountTableRow(name, numTransactions, transactionTable.ToArray());
            }

            // Construct TransferTable
            int numTransfers = transfers.Count;
            var transferTable = new TransferTableRow[numTransfers];

            for (int i = 0; i < numTransfers; i++)
            {
                var transfer = transfers[i];

                transferTable[i] = new TransferTableRow(
                    transfer.From.Name,
                    transfer.To.Name,
                    transfer.Number,
                    (decimal)transfer.Value,
                    transfer.Date.DayNumber,
                    transfer.Memo,
                    transfer.Category is not null ? transfer.Category.Name : "", // Use empty category name for null category
                    transfer.IsCleared
                    );
            }

            // Construct AccountBookFile and return
            return new AccountBookFile(
                numCategories,
                categoryTable,
                numAccounts,
                accountTable,
                numTransfers,
                transferTable
                );
        }
        
        /// <summary>
        /// Updates this instance of <see cref="AccountBookFile"/> with new information from an <see cref="AccountBook"/> instance.
        /// </summary>
        /// <param name="accountBook"></param>
        public void UpdateFrom(AccountBook accountBook)
        {
            // Create temporary file
            var tempAccBkFile = Deconstruct(accountBook);

            // Transfer data to this file
            NumCategories = tempAccBkFile.NumCategories;
            CategoryTable = tempAccBkFile.CategoryTable;
            NumAccounts = tempAccBkFile.NumAccounts;
            AccountTable = tempAccBkFile.AccountTable;
            NumTransfers = tempAccBkFile.NumTransfers;
            TransferTable   = tempAccBkFile.TransferTable;

            // Path in this AccountBookFile instance persists
        }

        /// <summary>
        /// Saves this instance of <see cref="AccountBookFile"/> to a new file at the given path. Overwrites any existing data.
        /// </summary>
        /// <param name="path"></param>
        public void SaveTo(string path)
        {
            // Open file (assume file can be overwritten)
            using var bw = new BinaryWriter(File.Create(path));

            // Write header and version no.
            bw.Write(Header);
            bw.Write(Version);

            // Write CategoryTable
            bw.Write(NumCategories);
            for (int i = 0; i < NumCategories; i++)
            {
                var categoryRow = CategoryTable[i];
                
                // Write Name
                bw.Write(categoryRow.Name);

                // Write IncomeBudget, if it exists
                bw.Write(categoryRow.HasIncomeBudget);
                if (categoryRow.HasIncomeBudget)
                {
                    bw.Write(categoryRow.IncomeBudget!.PerPeriod); // IncomeBudget cannot be null, since HasIncomeBudget => IncomeBudget is not null
                    bw.Write(categoryRow.IncomeBudget!.Period);
                }

                // Write ExpensesBudget, if it exists
                bw.Write(categoryRow.HasExpensesBudget);
                if (categoryRow.HasExpensesBudget)
                {
                    bw.Write(categoryRow.ExpensesBudget!.PerPeriod); // ExpensesBudget cannot be null, since HasExpensesBudget => ExpensesBudget is not null
                    bw.Write(categoryRow.ExpensesBudget!.Period);
                }
            }

            // Write AccountTable
            bw.Write(NumAccounts);
            for (int i = 0; i < NumAccounts; i++)
            {
                var accountRow = AccountTable[i];
                
                // Write Name
                bw.Write(accountRow.Name);

                // Write TransactionTable
                bw.Write(accountRow.NumTransactions);
                for (int j = 0; j < accountRow.NumTransactions; j++)
                {
                    var transactionRow = accountRow.TransactionTable[j];

                    bw.Write(transactionRow.TransactionNumber);
                    bw.Write(transactionRow.Value);
                    bw.Write(transactionRow.DayNumber);
                    bw.Write(transactionRow.Payee);
                    bw.Write(transactionRow.Memo);
                    bw.Write(transactionRow.CategoryName);
                    bw.Write(transactionRow.IsCleared);
                }
            }

            // Write TransferTable
            bw.Write(NumTransfers);
            for (int i = 0; i < NumTransfers; i++)
            {
                var transferRow = TransferTable[i];

                bw.Write(transferRow.FromAccName);
                bw.Write(transferRow.ToAccName);
                bw.Write(transferRow.TransactionNumber);
                bw.Write(transferRow.Value);
                bw.Write(transferRow.DayNumber);
                bw.Write(transferRow.Memo);
                bw.Write(transferRow.CategoryName);
                bw.Write(transferRow.IsCleared);
            }

            // Flush and close
            bw.Flush();
            bw.Close();
        }

        /// <summary>
        /// Constructs a new <see cref="AccountBook"/> from this instance of <see cref="AccountBookFile"/>.
        /// </summary>
        /// <returns></returns>
        public AccountBook Construct()
        {
            // Build Categories
            var categories = new Category[NumCategories];
            for (int i = 0; i < NumCategories; i++)
            {
                var categoryRow = CategoryTable[i];
                
                // Construct budgets (if they exist)
                Budget? incomeBudget =
                    categoryRow.HasIncomeBudget
                    ? new Budget(categoryRow.IncomeBudget!.PerPeriod, (Period)categoryRow.IncomeBudget!.Period)
                    : null;

                Budget? expensesBudget =
                    categoryRow.HasExpensesBudget
                    ? new Budget(categoryRow.ExpensesBudget!.PerPeriod, (Period)categoryRow.ExpensesBudget!.Period)
                    : null;

                // Construct category
                categories[i] = new Category(categoryRow.Name, incomeBudget, expensesBudget);
            }

            // Build Accounts w/ Transactions
            var accounts = new Account[NumAccounts];
            for (int i = 0; i < NumAccounts; i++)
            {
                var accountRow = AccountTable[i];
                
                // Construct Transactions
                var transactions = new Transaction[accountRow.NumTransactions];
                for (int j = 0; j < accountRow.NumTransactions; j++)
                {
                    var transactionRow = accountRow.TransactionTable[j];

                    transactions[j] = new Transaction(
                        transactionRow.Value,
                        DateOnly.FromDayNumber(transactionRow.DayNumber),
                        transactionRow.Payee,
                        transactionRow.Memo,
                        transactionRow.TransactionNumber
                        );

                    transactions[j].Category = categories.Where(x => x.Name == transactionRow.CategoryName).First();
                }

                // Construct account
                accounts[i] = new Account(accountRow.Name, transactions);
            }

            // Add Transfers
            foreach (TransferTableRow transferRow in TransferTable)
            {
                // Find relevant Accounts
                var fromAccount = accounts.Where(x => x.Name == transferRow.FromAccName).First();
                var toAccount = accounts.Where(x => x.Name == transferRow.ToAccName).First();

                // Create transfer
                fromAccount.TransferTo(
                    toAccount,
                    transferRow.Value,
                    DateOnly.FromDayNumber(transferRow.DayNumber),
                    transferRow.Memo,
                    transferRow.TransactionNumber,
                    categories.Where(x => x.Name == transferRow.CategoryName).First()
                    );
            }

            // Construct AccountBook, add components and return
            var ret = new AccountBook();
            ret.AddCategories(categories);
            ret.AddAccounts(accounts);
            return ret;
        }

    }
}
