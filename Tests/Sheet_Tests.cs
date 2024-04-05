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
    }
}
