using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Account_Tests
    {
        [TestMethod]
        public void ConstructionSorting_Test()
        {
            // Accounts sort transactions by date upon construction
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var transaction1 = new Transaction(0, "", today);
            var transaction2 = new Transaction(0, "", today.AddDays(1));
            var transaction3 = new Transaction(0, "", today.AddDays(2));
            Transaction[] transactions = [transaction3, transaction2, transaction1]; // Add transactions out of order

            // Act
            var account = new Account("", transactions);

            // Assert
            Assert.IsTrue(account.Transactions[0] == transaction1); // Check transactions are now in order
            Assert.IsTrue(account.Transactions[1] == transaction2);
            Assert.IsTrue(account.Transactions[2] == transaction3);
        }

        [TestMethod]
        public void NewTransaction_Tests()
        {
            // Adding a new transaction can throw some exceptions, and also orders transactions by date
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var duplicateTransaction = new Transaction(0, "", today.AddDays(3));
            var firstTransaction = new Transaction(0, "", today.AddDays(-50));
            var account = new Account("", duplicateTransaction, new Transaction(0, "", today), new Transaction(0, "", today.AddDays(1)), new Transaction(0, "", today.AddDays(1)));

            // Act and Assert
            Assert.ThrowsException<TransactionAlreadyExistsException>(() => { account.NewTransaction(duplicateTransaction); });
            account.NewTransaction(firstTransaction);
            Assert.AreEqual(account.Transactions[0], firstTransaction);
        }
    }
}
