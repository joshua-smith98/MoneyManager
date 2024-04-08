namespace MoneyManager.Core
{
    /// <summary>
    /// Represents a financial sheet containing Accounts and Categories.
    /// </summary>
    public class Sheet : Balanceable
    {
        public override Transaction[] Transactions => Accounts.SelectMany(x => x.Transactions).OrderBy(x => x.Date).ToArray();
        
        public Account[] Accounts => accounts.ToArray();
        private readonly List<Account> accounts = [];

        public Category[] Categories => categories.ToArray();
        private readonly List<Category> categories = [];

        public Sheet() { }

        /// <summary>
        /// Adds the given <see cref="Account"/> to this Sheet.
        /// Throws a <see cref="SheetException"/> if it already exists within this Sheet.
        /// </summary>
        /// <param name="account"></param>
        /// <exception cref="SheetException"></exception>
        public void NewAccount(Account account)
        {
            // Validity check: Accounts must not already contain account
            if (Accounts.Contains(account)) throw new SheetException($"Account \"{account.Name}\" already exists in sheet.");

            accounts.Add(account);
        }

        /// <summary>
        /// Adds the given Accounts to this Sheet.
        /// Throws a <see cref="SheetException"/> if any of the them already exist in this Sheet.
        /// </summary>
        /// <param name="accounts"></param>
        /// <exception cref="SheetException"></exception>
        public void NewAccounts(params Account[] accounts)
        {
            // Validity check: Accounts must not already contain any of accounts
            foreach (Account account in accounts) // Use a foreach here rather than linq so we can access the account name
                if (Accounts.Contains(account)) throw new SheetException($"Account \"{account.Name}\" already exists in sheet.");

            this.accounts.AddRange(accounts);
        }

        /// <summary>
        /// Removes the given <see cref="Account"/> from this Sheet.
        /// </summary>
        /// <param name="account"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void DeleteAccount(Account account)
        {
            // Validity check: account must be within Accounts
            if (!Accounts.Contains(account)) throw new IndexOutOfRangeException();

            accounts.Remove(account);
        }

        /// <summary>
        /// Removes the <see cref="Account"/> in this Sheet at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void DeleteAccountAt(int index)
        {
            // Validity check: index must be within the range of accounts
            if (index < 0 || index >= accounts.Count) throw new IndexOutOfRangeException();

            accounts.RemoveAt(index);
        }

        /// <summary>
        /// Adds the given <see cref="Category"/> to this Sheet.
        /// Throws a <see cref="SheetException"/> if the given Category already exists within this Sheet.
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="SheetException"></exception>
        public void NewCategory(Category category)
        {
            // Validity check: category must not already be in Categories
            if (Categories.Contains(category)) throw new SheetException($"Category \"{category.Name}\" already exists in sheet.");

            categories.Add(category);
        }

        /// <summary>
        /// Adds the given Categories to this Sheet.
        /// Throws a <see cref="SheetException"/> if any of them already exist within this Sheet.
        /// </summary>
        /// <param name="categories"></param>
        /// <exception cref="SheetException"></exception>
        public void NewCategories(params Category[] categories)
        {
            // Validity check: Categories must not already contain any of categories
            foreach (Category category in categories) // Use a foreach here rather than linq so we can access the category name
                if (Categories.Contains(category)) throw new SheetException($"Category \"{category.Name}\" already exists in sheet.");

            this.categories.AddRange(categories);
        }

        /// <summary>
        /// Removes the given <see cref="Category"/> from this Sheet, and detaches all <see cref="Transaction"/>s contained in that Category.
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void DeleteCategory(Category category)
        {
            // Validity check: category must be contained within Categories
            if (!Categories.Contains(category)) throw new IndexOutOfRangeException();

            categories.Remove(category);

            // Remove all references to this category from its transactions
            // PURGE IT FROM ALL SPACETIME!!
            foreach (Transaction transaction in category.Transactions) transaction.Category = null;
        }

        /// <summary>
        /// Removes the <see cref="Category"/> at the given index from this Sheet, and detaches all <see cref="Transaction"/>s contained in that Category.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void DeleteCategoryAt(int index)
        {
            // Validity check: index must be within the bounds of categories
            if (index < 0 ||index >= categories.Count) throw new IndexOutOfRangeException();

            DeleteCategory(categories[index]); // Avoid duplicating purging code
        }

        public ReportChunk GenerateReportChunk(DateOnly startDate, Period period)
        {
            // Generate ReportChunkCategories
            List<ReportChunkCategory> reportChunkCategories = [];

            foreach (Category category in Categories)
            {
                var reportChunkCategory = new ReportChunkCategory(category, startDate, period, category.BalanceInfoForPeriod(startDate, period))
                {
                    IncomeBudget = category.IncomeBudget,
                    ExpensesBudget = category.ExpensesBudget,
                    IncomeDifference = category.GetIncomeDifference(startDate, period),
                    ExpensesDifference = category.GetExpensesDifference(startDate, period)
                };
                reportChunkCategories.Add(reportChunkCategory);
            }

            // Generate ReportChunk and Return
            return new ReportChunk(
                startDate,
                period,
                BalanceInfoForPeriod(startDate, period),
                BalanceInfoAtDate(period.GetEndDateInclusive(startDate)),
                reportChunkCategories.ToArray()
                );
        }

        public ReportStepped GenerateReportStepped(DateOnly startDate, Period period, Period stepPeriod)
        {
            // Generate ReportChunks
            int numReportChunks = period.DivideIntoOrNull(stepPeriod) ?? throw new SheetException("Can't generate a stepped report with Periods the don't divide evenly.");
            ReportChunk[] reportChunks = new ReportChunk[numReportChunks];
            DateOnly currentDate = startDate;

            for (int i = 0; i < numReportChunks; i++)
            {
                reportChunks[i] = GenerateReportChunk(currentDate, stepPeriod);
                currentDate = stepPeriod.GetEndDateExclusive(currentDate);
            }

            // Generate ReportStepped and Return
            return new ReportStepped(startDate, period, stepPeriod, reportChunks);
        }
    }
}
