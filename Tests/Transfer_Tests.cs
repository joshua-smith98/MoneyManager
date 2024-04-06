using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Transfer_Tests
    {
        [TestMethod]
        public void Metadata_Tests()
        {
            // Each basic metadata setter will set the value of twin as well
            // Testing: Number, Date, Memo, Category

            // Arrange

            // Create and retrieve transfer
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));
            account1.TransferTo(account2, 200);
            Transaction transfer1 = account1.Transactions[^1]; // Test that it works on transactions that are actually transfers too
            Transaction transfer2 = account2.Transactions[^1];

            // Act
            transfer1.Number = "12345";
            transfer1.Date = DateOnly.FromDateTime(DateTime.Now).AddDays(50);
            transfer1.Memo = "678910";
            transfer1.Category = new Category("");

            // Assert
            // Check the values have actually been set
            Assert.AreNotEqual(transfer2.Number, string.Empty);
            Assert.AreNotEqual(transfer2.Date, DateOnly.FromDateTime(DateTime.Today));
            Assert.AreNotEqual(transfer2.Memo, string.Empty);
            Assert.AreNotEqual(transfer2.Category, null);

            // Check values are equal
            Assert.AreEqual(transfer1.Number, transfer2.Number);
            Assert.AreEqual(transfer1.Date, transfer2.Date);
            Assert.AreEqual(transfer1.Memo, transfer2.Memo);
            Assert.AreEqual(transfer1.Category, transfer2.Category);

        }

        [TestMethod]
        public void Value_Test()
        {
            // Will change the value in the twin to the negative of the given value
            // Throws exception if given value is not the same sign as current
            // Arrange

            // Create and retrieve transfer
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));
            account1.TransferTo(account2, 200);
            Transaction transfer1 = account1.Transactions[^1]; // Test that it works on transactions that are actually transfers too
            Transaction transfer2 = account2.Transactions[^1];

            // Act
            transfer1.Value = -100;
            Assert.AreEqual(transfer2.Value, 100);

            // Assert
            Assert.ThrowsException<TransferException>(() => transfer1.Value = 100);
        }

        [TestMethod]
        public void Payee_Test()
        {
            // Automatically shows [From X] if transaction is in the To account, or [To y] if transaction is in the from account
            // Arrange
            // Create transfers
            var account1 = new Account("FromAccount");
            var account2 = new Account("ToAccount");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));
            account1.TransferTo(account2, 200);

            // Act and Assert
            Assert.AreEqual(account1.Transactions[^1].Payee, "[To ToAccount]");
            Assert.AreEqual(account2.Transactions[^1].Payee, "[From FromAccount]");
        }

        [TestMethod]
        public void Create_Test()
        {
            // Creates a new transfer between the given accounts, with the given metadata. The Value in the From account should be negative, and positive in the To account.
            // Arrange

            // Create and retrieve transfers
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));

            // Act
            Transfer.Create(account1, account2, 200);

            // Assert
            Assert.AreEqual(account1.Transactions[^1].Value, -200); // Check value of From is negative
            Assert.AreEqual(account2.Transactions[^1].Value, 200); // Check value of To is positive
        }

        [TestMethod]
        public void Delete_Test()
        {
            // Removes the given transfer and its twin from their respective accounts, and detaches any categories
            // Arrange
            // Create and retrieve transfer
            var category = new Category("");
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));
            account1.TransferTo(account2, 200);
            Transfer transfer1 = (Transfer)account1.Transactions[^1];
            Transfer transfer2 = (Transfer)account2.Transactions[^1];
            transfer1.Category = category;

            // Act
            transfer1.Delete();

            // Assert
            Assert.IsFalse(account1.Transactions.Contains(transfer1));
            Assert.IsFalse(account2.Transactions.Contains(transfer2));
            Assert.AreEqual(transfer1.Category, null);
            Assert.AreEqual(transfer2.Category, null);
        }
    }
}
