using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class BalanceInfo_Tests
    {
        [TestMethod]
        public void EmptyTransactions_Test()
        {
            // All BalanceInfo values are 0 when constructed with an empty transaction array
            // Arrange
            var balanceInfo = new BalanceInfo([]);

            // Assert
            Assert.AreEqual(balanceInfo.Balance, 0);
            Assert.AreEqual(balanceInfo.Income, 0);
            Assert.AreEqual(balanceInfo.Expenses, 0);
            Assert.AreEqual(balanceInfo.ClearedBalance, 0);
            Assert.AreEqual(balanceInfo.ClearedIncome, 0);
            Assert.AreEqual(balanceInfo.ClearedExpenses, 0);
        }

        public void Balance_Tests()
        {
            // The constructor calculates Balance and ClearedBalance as the sum of each Transaction's Value and ClearedValue
            // Arrange
            Money income1 = 50;
            Money income2 = 25;
            Money expenses1 = -100;
            Money expenses2 = -10;
            Money unclearedIncome = 732.92m;
            Money unclearedExpenses = -35.22m;

            Money balance = income1 + income2 + expenses1 + expenses2 + unclearedIncome + unclearedExpenses;
            Money clearedBalance = income1 + income2 + expenses1 + expenses2;
            Money income = income1 + income2 + unclearedIncome;
            Money clearedIncome = income1 + income2;
            Money expenses = expenses1 + expenses2 + unclearedExpenses;
            Money clearedExpenses = expenses1 + expenses2;

            Transaction[] transactions = [
                new Transaction(income1, ""),
                new Transaction(income2, ""),
                new Transaction(expenses1, ""),
                new Transaction(expenses2, ""),
                new Transaction(unclearedIncome, ""){ IsCleared = false },
                new Transaction(unclearedExpenses, ""){ IsCleared = false }
                ];

            // Act
            var balanceInfo = new BalanceInfo(transactions);

            // Assert
            Assert.AreEqual(balanceInfo.Balance, balance);
            Assert.AreEqual(balanceInfo.ClearedBalance, clearedBalance);
            Assert.AreEqual(balanceInfo.Income, income);
            Assert.AreEqual(balanceInfo.ClearedIncome, clearedIncome);
            Assert.AreEqual(balanceInfo.Expenses, expenses);
            Assert.AreEqual(balanceInfo.ClearedExpenses, clearedExpenses);
        }
    }
}
