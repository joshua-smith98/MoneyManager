using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Sheet_Tests
    {
        [TestMethod]
        public void NewAccount_Tests()
        {
            // - Adds a new account to the accounts array
            // - Throws a SheetException if the account is already in the array

            // Arrange
            var account = new Account("");
            var sheet = new Sheet();

            // Act and Assert
            sheet.NewAccount(account);
            Assert.IsTrue(sheet.Accounts.Contains(account));
            Assert.ThrowsException<SheetException>(() => sheet.NewAccount(account)); // Check for duplicate account exception
        }

        [TestMethod]
        public void NewAccounts_Tests()
        {
            // - Adds the given accounts to the accounts array
            // - Throws a SheetException if any of the accounts are already in the array

            // Arrange
            Account[] outsideAccounts = [
                new Account("account1"),
                new Account("account2"),
                ];
            Account[] insideAccounts = [
                new Account("account3"),
                new Account("account4")
                ];
            Account[] allAccounts = [.. outsideAccounts, .. insideAccounts];
            var sheet = new Sheet();

            // Act and Assert
            sheet.NewAccounts(allAccounts);
            Assert.IsTrue(allAccounts.All(x => sheet.Accounts.Contains(x))); // Check that all accounts are in the sheet
            Assert.ThrowsException<SheetException>(() => sheet.NewAccounts(insideAccounts)); // Check for duplicate account exception
        }


        [TestMethod]
        public void DeleteAccount_Tests()
        {
            // Removes the given account from the sheet
            // Throws an IndexOutOfRangeException if the given account is not in the sheet

            // Arrange
            var sheet = new Sheet();
            var outsideAccount = new Account("outside account");
            Account[] accounts = [
                new Account("account1"),
                new Account("account2"),
                new Account("account3"),
                new Account("account4")
                ];
            sheet.NewAccounts(accounts);

            // Act and Assert
            sheet.DeleteAccount(accounts[0]);
            Assert.IsFalse(sheet.Accounts.Contains(accounts[0])); // Check account was removed from the sheet
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteAccount(outsideAccount)); // Check IndexOutOfRangeException
        }

        [TestMethod]
        public void DeleteAccountAt_Tests()
        {
            // Removes the account at the given index from the sheet
            // Throws an IndexOutOfRangeException if the index is out of range

            // Arrange
            var sheet = new Sheet();
            Account[] accounts = [
                new Account("account1"),
                new Account("account2"),
                new Account("account3"),
                new Account("account4")
                ];
            sheet.NewAccounts(accounts);

            // Act and Assert
            sheet.DeleteAccountAt(2);
            Assert.IsFalse(sheet.Accounts.Contains(accounts[2])); // Check account was removed from the sheet
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteAccountAt(-5)); // Check IndexOutOfRangeException
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteAccountAt(20)); // Check IndexOutOfRangeException
        }

        [TestMethod]
        public void NewCategory_Tests()
        {
            // - Adds a new category to the accounts array
            // - Throws a SheetException if the category is already in the array

            // Arrange
            var category = new Category("");
            var sheet = new Sheet();

            // Act and Assert
            sheet.NewCategory(category);
            Assert.IsTrue(sheet.Categories.Contains(category));
            Assert.ThrowsException<SheetException>(() => sheet.NewCategory(category)); // Check for duplicate category exception
        }

        [TestMethod]
        public void NewCategories_Tests()
        {
            // - Adds the given categories to the accounts array
            // - Throws a SheetException if any of the categories are already in the array

            // Arrange
            Category[] outsideCategories = [
                new Category("account1"),
                new Category("account2"),
                ];
            Category[] insideCategories = [
                new Category("account3"),
                new Category("account4")
                ];
            Category[] allCategories = [.. outsideCategories, .. insideCategories];
            var sheet = new Sheet();

            // Act and Assert
            sheet.NewCategories(allCategories);
            Assert.IsTrue(allCategories.All(x => sheet.Categories.Contains(x))); // Check that all categories are in the sheet
            Assert.ThrowsException<SheetException>(() => sheet.NewCategories(insideCategories)); // Check for duplicate category exception
        }


        [TestMethod]
        public void DeleteCategory_Tests()
        {
            // Removes the given category from the sheet
            // Removes the given category from any of its transactions
            // Throws an IndexOutOfRangeException if the given category is not in the sheet

            // Arrange
            var sheet = new Sheet();
            var outsideCategory = new Category("outside category");
            var insideCategory = new Category("inside category");
            Category[] categories = [
                new Category("account1"),
                new Category("account2"),
                new Category("account3"),
                new Category("account4"),
                insideCategory
                ];
            sheet.NewCategories(categories);
            var transaction = new Transaction(1, "") { Category = insideCategory };

            // Act and Assert
            sheet.DeleteCategory(insideCategory);
            Assert.IsFalse(sheet.Categories.Contains(insideCategory)); // Check category was removed from the sheet
            Assert.IsNull(transaction.Category); // Check transaction had its category removed
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteCategory(outsideCategory)); // Check IndexOutOfRangeException
        }

        [TestMethod]
        public void DeleteCategoryAt_Tests()
        {
            // Throws an IndexOutOfRangeException if the index is out of range
            // Calls DeleteCategory(), so we don't need to test anything else

            // Arrange
            var sheet = new Sheet();
            Category[] categories = [
                new Category("account1"),
                new Category("account2"),
                new Category("account3"),
                new Category("account4")
                ];
            sheet.NewCategories(categories);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteCategoryAt(-5)); // Check IndexOutOfRangeException
            Assert.ThrowsException<IndexOutOfRangeException>(() => sheet.DeleteCategoryAt(20)); // Check IndexOutOfRangeException
        }

        [TestMethod]
        public void GenerateReportChunk_Tests()
        {
            // Returns a new ReportChunk, containing ReportChunkCategories
            // Arrange
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var period = Period.Weekly;

            var sheet = new Sheet();
            var category1 = new Category("Category 1")
            {
                IncomeBudget = new Budget(20, period),
                ExpensesBudget  = new Budget(-35, period)
            };
            var category2 = new Category("Category 2")
            {
                IncomeBudget = new Budget(500, period),
                ExpensesBudget = new Budget(-55, period)
            };
            sheet.NewCategory(category1);
            sheet.NewCategory(category2);

            Transaction[] transactionsBeforePeriod = [
                new Transaction(50, startDate.AddDays(-5)),
                new Transaction(-70, startDate.AddDays(-2)),
                ];

            Transaction[] transactions = [
                new Transaction(25, startDate) {Category = category1},
                new Transaction(-30, startDate.AddDays(5)) {Category = category1},
                new Transaction(523, startDate.AddDays(3)) {Category = category2},
                new Transaction(-60, period.GetEndDateInclusive(startDate)) {Category = category2},
                new Transaction(-367, startDate.AddDays(2)), // Test no category
                new Transaction(20, startDate.AddDays(4)),
                ];

            Transaction[] transactionsAfterPeriod = [
                new Transaction(77, startDate.AddDays(8)),
                new Transaction(-57, startDate.AddDays(25)),
                ];

            sheet.NewAccount(new Account("Account", [.. transactionsBeforePeriod, .. transactions, .. transactionsAfterPeriod]));

            // Act
            var reportChunk = sheet.GenerateReportChunk(startDate, period);

            // Assert
            // Check BalanceInfo
            Assert.AreEqual(reportChunk.BalanceInfo.Income, 25 + 523 + 20);
            Assert.AreEqual(reportChunk.BalanceInfo.Expenses, -30 - 60 - 367);
            Assert.AreEqual(reportChunk.BalanceInfo.Balance, 25 + 523 + 20 - 30 - 60 - 367);

            // Check BalanceInfoToDate
            Assert.AreEqual(reportChunk.BalanceInfoToDate.Income, 50 + 25 + 523 + 20);
            Assert.AreEqual(reportChunk.BalanceInfoToDate.Expenses, -70 - 30 - 60 - 367);
            Assert.AreEqual(reportChunk.BalanceInfoToDate.Balance, 50 + 25 + 523 + 20 - 70 - 30 - 60 - 367);

            // Check Categories exist
            Assert.IsTrue(sheet.Categories.All(reportChunk.ReportChunkCategories.ContainsKey));
            Assert.IsTrue(reportChunk.ReportChunkCategories.ContainsKey(null)); // Check for uncategorised transactions

            // Check category1
            // Check BalanceInfo
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BalanceInfo.Income, 25);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BalanceInfo.Expenses, -30);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BalanceInfo.Balance, 25 - 30);

            // Check Budgets
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BudgetedIncome, 20);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BudgetedExpenses, -35);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BudgetedBalance, 20 - 35);

            // Check Differences
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].IncomeDifference, 25 - 20);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].ExpensesDifference, -30 - -35);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category1].BalanceDifference, (25 - 20) + (-30 - -35));

            // Check category2
            // Check BalanceInfo
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BalanceInfo.Income, 523);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BalanceInfo.Expenses, -60);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BalanceInfo.Balance, 523 - 60);

            // Check Budgets
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BudgetedIncome, 500);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BudgetedExpenses, -55);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BudgetedBalance, 500 - 55);

            // Check Differences
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].IncomeDifference, 523 - 500);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].ExpensesDifference, -60 - -55);
            Assert.AreEqual(reportChunk.ReportChunkCategories[category2].BalanceDifference, (523 - 500) + (-60 - -55));

            // Check null category
            // Check BalanceInfo
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BalanceInfo.Income, 20);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BalanceInfo.Expenses, -367);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BalanceInfo.Balance, 20 - 367);

            // Check Budgets
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BudgetedIncome, null);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BudgetedExpenses, null);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BudgetedBalance, null);

            // Check Differences
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].IncomeDifference, null);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].ExpensesDifference, null);
            Assert.AreEqual(reportChunk.ReportChunkCategories[null].BalanceDifference, null);
        }

        [TestMethod]
        public void GenerateReportStepped_Tests()
        {
            // Generates a stepped report from the given start date, across the given period and with steps of the given period
            // Throws a SheetException if the period and step period are incompatible (not evenly divisible)
            // We won't bother to check the ReportChunks here since we did it in the above method, except for the startdates, enddates and periods

            // Arrange
            var random = new Random();
            var period = Period.Annually;
            var stepPeriod = Period.Daily;
            var startDate = DateOnly.FromDateTime(DateTime.Today);
            var endDate = period.GetEndDateExclusive(startDate);

            Transaction[] preTransactions = [
                new Transaction(50, startDate.AddDays(-5)),
                new Transaction(-35, startDate.AddMonths(-5))
                ];
            Transaction[] postTransactions = [
                new Transaction(70, endDate.AddDays(5)),
                new Transaction(-20, endDate.AddMonths(5))
                ];

            var stepDate = startDate;
            Transaction[] transactions = new Transaction[365];
            for (int i = 0; i < transactions.Length; i++)
            {
                var val = random.Next(-500, 500);
                while (val == 0) val = random.Next(-500, 500);
                transactions[i] = new Transaction(val, stepDate);
                stepDate = stepDate.AddDays(1);
            }

            var account = new Account("Account", [.. preTransactions, .. transactions, .. postTransactions]);
            var sheet = new Sheet();
            sheet.NewAccount(account);

            // Act
            var reportStepped = sheet.GenerateReportStepped(startDate, period, stepPeriod);

            // Assert
            // We won't bother checking the total report chunk, because we already know this will work from the previous test
            // Check ReportChunks
            stepDate = startDate;
            for(int i = 0; i < reportStepped.ReportChunks.Length; i++)
            {
                Assert.AreEqual(stepDate, reportStepped.ReportChunks[i].StartDate);
                Assert.AreEqual(stepDate, reportStepped.ReportChunks[i].EndDate); // Report end dates are inclusive, so in this case they're the same as the stepdate
                Assert.AreEqual(stepPeriod, reportStepped.ReportChunks[i].Period);
                Assert.AreEqual(transactions[i].Value, reportStepped.ReportChunks[i].BalanceInfo.Balance); // Check the report chunk matches the given transaction
                stepDate = stepDate.AddDays(1);
            }

            // Check exception upon incompatible periods being given
            Assert.ThrowsException<SheetException>(() => reportStepped = sheet.GenerateReportStepped(startDate, Period.Monthly, Period.Fortnightly)); // Test incompatible periods
            Assert.ThrowsException<SheetException>(() => reportStepped = sheet.GenerateReportStepped(startDate, Period.Fortnightly, Period.Monthly)); // Step period larger than period
        }
    }
}
