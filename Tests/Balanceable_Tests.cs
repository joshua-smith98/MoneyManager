using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Balanceable_Tests
    {
        [TestMethod]
        public void BalanceInfoAt_Tests()
        {
            // Gets the BalanceInfo for all transactions up to and including the given transaction
            // Throws IndexOutOfRangeException if the given transaction isn't in the Balanceable instance
            // Arrange
            var outsideTransaction = new Transaction(50, "");
            var transactionUntil = new Transaction(10, "");
            Transaction[] transactions = [
                    new Transaction(5, ""),
                    transactionUntil,
                    new Transaction(-20, "")
                ];
            var account = new Account("");
            account.NewTransactions(transactions);
            var actualBalanceInfo = new BalanceInfo(transactions[0..2]);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoAt(outsideTransaction));
            Assert.AreEqual(account.BalanceInfoAt(transactionUntil), actualBalanceInfo);
        }

        [TestMethod]
        public void BalanceInfoAtIndex_Tests()
        {
            // Gets the BalanceInfo for all transactions up to and including the given index
            // Throws IndexOutOfRangeException if the given index is outside the bounds of the array
            // Arrange
            Transaction[] transactions = [
                    new Transaction(5, ""),
                    new Transaction(10, ""),
                    new Transaction(-20, "")
                ];
            var account = new Account("");
            account.NewTransactions(transactions);
            var actualBalanceInfo = new BalanceInfo(transactions[0..2]);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoAtIndex(-1)); // Negative index
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoAtIndex(10)); // Out of range index
            Assert.AreEqual(account.BalanceInfoAtIndex(1), actualBalanceInfo);
        }

        [TestMethod]
        public void BalanceInfoBetween_Tests()
        {
            // Gets the BalanceInfo for all transactions between and including the given transactions
            // Throws IndexOutOfRangeException if either of the given transactions isn't in the Balanceable instance
            // Arrange
            var fromTransaction = new Transaction(-2, "");
            var toTransaction = new Transaction(34, "");
            var outsideTransaction = new Transaction(0, "");
            Transaction[] transactions = [
                    new Transaction(5, ""),
                    fromTransaction,
                    new Transaction(10, ""),
                    toTransaction,
                    new Transaction(-20, "")
                ];
            var account = new Account("");
            account.NewTransactions(transactions);
            var actualBalanceInfo = new BalanceInfo(transactions[1..4]);

            // Act and Assert
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoBetween(outsideTransaction, toTransaction));
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoBetween(fromTransaction, outsideTransaction));
            Assert.AreEqual(account.BalanceInfoBetween(fromTransaction, toTransaction), actualBalanceInfo);
        }

        [TestMethod]
        public void BalanceInfoBetweenIndices_Tests()
        {
            // Gets the BalanceInfo for all transactions between and including the given indicies
            // Throws ArgumentOutOfRangeException if 'from' is larger than or equal to 'to'
            // Throws IndexOutOfRangeException if either of the given indices are outside the bounds of the array
            // Arrange
            Transaction[] transactions = [
                    new Transaction(5, ""),
                    new Transaction(-2, ""),
                    new Transaction(10, ""),
                    new Transaction(34, ""),
                    new Transaction(-20, "")
                ];
            var account = new Account("");
            account.NewTransactions(transactions);
            var actualBalanceInfo = new BalanceInfo(transactions[1..4]);

            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => account.BalanceInfoBetweenIndices(5, 4));
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoBetweenIndices(-1, 9));
            Assert.ThrowsException<IndexOutOfRangeException>(() => account.BalanceInfoBetweenIndices(3, 9));
            Assert.AreEqual(account.BalanceInfoBetweenIndices(1, 3), actualBalanceInfo);
        }

        [TestMethod]
        public void BalanceInfoForPeriod_Tests()
        {
            // Gets the BalanceInfo for all the transactions within the given period, starting from the given date
            // Throws ArgumentOutOfRangeException if period is Period.Null
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var account = new Account("");

            Transaction[] transactionsInPeriod = [
                new Transaction(-123, "", today),
                new Transaction(-234, "", today),
                new Transaction(345, "", today),
                new Transaction(-456, "", today)
                ];
            Transaction[] transactionsOutsidePeriod = [
                new Transaction(-321, "", today.AddDays(-1)),
                new Transaction(-432, "", today.AddDays(1)),
                new Transaction(-543, "", today.AddYears(1)),
                new Transaction(654, "", today.AddMonths(5))
                ];
            account.NewTransactions([.. transactionsInPeriod, .. transactionsOutsidePeriod]);

            var actualBalanceInfo = new BalanceInfo(transactionsInPeriod);

            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => account.BalanceInfoForPeriod(today, Period.Null));
            Assert.AreEqual(account.BalanceInfoForPeriod(today, Period.Daily), actualBalanceInfo);
        }
    }
}
