
using MoneyManager.Core;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class Transaction_Tests
    {
        [TestMethod]
        public void ClearedValue_Test()
        {
            // Arrange
            Money value = 10;
            var clearedT = new Transaction(value, ""); // Should be cleared by default
            var unclearedT = new Transaction(value, "") { IsCleared = false };

            // Act & Assert
            Assert.AreEqual(clearedT.ClearedValue, value);
            Assert.AreEqual(unclearedT.ClearedValue, 0);
        }

        [TestMethod]
        public void Category_Test()
        {
            // Category setter automatically adds and removes transaction from that category
            // Arrange
            var cat1 = new Category("cat1");
            var cat2 = new Category("cat2");

            var transaction = new Transaction(1, "");

            // Act and Assert
            transaction.Category = cat1;
            Assert.IsTrue(cat1.Transactions.Contains(transaction)); // Added to cat1

            transaction.Category = cat2;
            Assert.IsFalse(cat1.Transactions.Contains(transaction)); // Removed from cat1
            Assert.IsTrue(cat2.Transactions.Contains(transaction)); // Added to cat2

            transaction.Category = null;
            Assert.IsFalse(cat2.Transactions.Contains(transaction)); // Removed from cat2
        }

        [TestMethod]
        public void TransactionType_Test()
        {
            // Transaction type must be 'Withdrawal' for negative values and 'Deposit' for positive values
            // Arrange
            var withdrawalT = new Transaction(-10, "");
            var depositT = new Transaction(10, "");

            // Act and Assert
            Assert.IsTrue(withdrawalT.TransactionType is TransactionType.Withdrawal);
            Assert.IsTrue(depositT.TransactionType is TransactionType.Deposit);
        }
    }
}
