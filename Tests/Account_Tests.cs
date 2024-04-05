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
            var transaction1 = new Transaction(0, today);
            var transaction2 = new Transaction(0, today.AddDays(1));
            var transaction3 = new Transaction(0, today.AddDays(2));
            Transaction[] transactions = [transaction3, transaction2, transaction1]; // Add transactions out of order

            // Act
            var account = new Account("", transactions);

            // Assert
            Assert.AreSame(account.Transactions[0], transaction1); // Check transactions are now in order
            Assert.AreSame(account.Transactions[1], transaction2);
            Assert.AreSame(account.Transactions[2], transaction3);
        }

        [TestMethod]
        public void NewTransaction_Tests()
        {
            // Adding a new transaction can throw some exceptions, and also orders transactions by date
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var duplicateTransaction = new Transaction(0, today.AddDays(3));
            var firstTransaction = new Transaction(0, today.AddDays(-50));
            var account = new Account("", duplicateTransaction, new Transaction(0, today), new Transaction(0, today.AddDays(1)), new Transaction(0, today.AddDays(1)));

            // Aquire transfer
            var account1 = new Account("");
            var account2 = new Account("");
            account1.TransferTo(account2, 10);
            var transfer = account1.Transactions[0];

            // Act and Assert
            Assert.ThrowsException<TransactionInvalidException>(() => { account.NewTransaction(transfer); }); // Test adding transfer
            Assert.ThrowsException<TransactionAlreadyExistsException>(() => { account.NewTransaction(duplicateTransaction); }); // Test duplicate transaction
            
            account.NewTransaction(firstTransaction);
            Assert.AreSame(account.Transactions[0], firstTransaction); // Test sorting
        }

        [TestMethod]
        public void NewTransactions_Tests()
        {
            // Adding new transactions can throw some exceptions, and also orders transactions by date
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var duplicateTransaction = new Transaction(0, today.AddDays(3));
            var firstTransaction = new Transaction(0, today.AddDays(-50));
            var account = new Account("", duplicateTransaction);

            // Aquire transfer
            var account1 = new Account("");
            var account2 = new Account("");
            account1.TransferTo(account2, 10);
            var transfer = account1.Transactions[0];

            Transaction[] transactions = [new Transaction(0, today), new Transaction(0, today.AddDays(1)), new Transaction(0, today.AddDays(1))];
            Transaction[] ts_with_duplicate = [.. transactions, duplicateTransaction];
            Transaction[] ts_with_transfer = [.. transactions, transfer];
            Transaction[] ts_with_first = [.. transactions, firstTransaction];

            // Act and Assert
            Assert.ThrowsException<TransactionInvalidException>(() => { account.NewTransactions(ts_with_transfer); }); // Test adding transfer
            Assert.ThrowsException<TransactionAlreadyExistsException>(() => { account.NewTransactions(ts_with_duplicate); }); // Test duplicate transaction

            account.NewTransactions(ts_with_first);
            Assert.AreSame(account.Transactions[0], firstTransaction); // Test sorting
        }

        [TestMethod]
        public void DeleteTransaction_Tests()
        {
            // DeleteTransaction:
            //  - Deletes the given transaction and clears its category
            //  - Throws IndexOutOfRangeException if the given transaction isn't in the account

            // DeleteTransaction also does things with transfers, but we're changing that soon so we'll leave it

            // Arrange
            var category = new Category("");
            var insideTransaction = new Transaction(0) { Category = category };
            var outsideTransaction = new Transaction(0);
            var account = new Account("", insideTransaction);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => { account.DeleteTransaction(outsideTransaction); }); // Check IndexOutOfRangeException
            account.DeleteTransaction(insideTransaction);
            Assert.IsFalse(account.Transactions.Contains(insideTransaction)); // Check deletion
            Assert.IsTrue(insideTransaction.Category is null); // Check category removal
        }

        [TestMethod]
        public void DeleteTransactionAt_Tests()
        {
            // - Throws IndexOutOfRangeException if given index is outside range of Transactions[]
            // We won't test the rest, because then it just calls DeleteTransaction()

            // Arrange
            var account = new Account("");
            account.NewTransactions(new Transaction(0), new Transaction(0));

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.DeleteTransactionAt(2)); // Check edge
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.DeleteTransactionAt(5)); // Check out of range
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.DeleteTransactionAt(-1)); // Check negative
        }

        [TestMethod]
        public void TransferTo_Tests()
        {
            // Transfers a given amount of money to another account.
            // Arrange
            var startAmount1 = 300;
            var startAmount2 = 300;
            var transferAmount = 200;
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(startAmount1, "[Initial Balance]"));
            account2.NewTransaction(new Transaction(startAmount2, "[Initial Balance]"));

            // Act
            account1.TransferTo(account2, transferAmount);

            // Assert
            Assert.AreEqual(startAmount1 - transferAmount, account1.BalanceInfo.Balance); // Check account 1 balance
            Assert.AreEqual(startAmount2 + transferAmount, account2.BalanceInfo.Balance); // Check account 2 balance
        }

        [TestMethod]
        public void TransferFrom_Tests()
        {
            // Transfers a given amount of money from another account to this one
            // Arrange
            var startAmount1 = 300;
            var startAmount2 = 300;
            var transferAmount = 200;
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(startAmount1, "[Initial Balance]"));
            account2.NewTransaction(new Transaction(startAmount2, "[Initial Balance]"));

            // Act
            account1.TransferFrom(account2, transferAmount);

            // Assert
            Assert.AreEqual(startAmount1 + transferAmount, account1.BalanceInfo.Balance); // Check account 1 balance
            Assert.AreEqual(startAmount2 - transferAmount, account2.BalanceInfo.Balance); // Check account 2 balance
        }
    }
}
