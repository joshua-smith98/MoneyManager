using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Category_Tests
    {
        [TestMethod]
        public void GetIncomeDifference_Tests()
        {
            // - Gets the difference between the actual income in a certain period, and the budgeted income
            // - Throws a CategoryException if IncomeBudget is null
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var account = new Account("");
            var category = new Category("");
            category.IncomeBudget = new Budget(500, Period.Daily);

            Transaction[] transactionsInPeriod = [
                new Transaction(123, "", today) { Category = category },
                new Transaction(234, "", today) { Category = category },
                new Transaction(-345, "", today) { Category = category },
                new Transaction(456, "", today)
                ];
            Transaction[] transactionsOutsidePeriod = [
                new Transaction(321, "", today.AddDays(-1)) { Category = category },
                new Transaction(432, "", today.AddDays(1)) { Category = category },
                new Transaction(543, "", today.AddYears(1)),
                new Transaction(-654, "", today.AddMonths(5)) { Category = category }
                ];
            account.NewTransactions([.. transactionsInPeriod, .. transactionsOutsidePeriod]);

            Money income = 123 + 234;
            Money actualDifference = income - 500;

            // Act and Assert
            Assert.AreEqual(actualDifference, category.GetIncomeDifference(today, Period.Daily));
            category.IncomeBudget = null;
            Assert.ThrowsException<CategoryException>(() => { category.GetIncomeDifference(today, Period.Daily); });
        }

        [TestMethod]
        public void GetExpensesDifference_Test()
        {
            // - Gets the difference between the actual expenses in a certain period, and the budgeted expenses
            // - Throws a CategoryException if ExpensesBudget is null
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var account = new Account("");
            var category = new Category("");
            category.ExpensesBudget = new Budget(-500, Period.Daily);

            Transaction[] transactionsInPeriod = [
                new Transaction(-123, "", today) { Category = category },
                new Transaction(-234, "", today) { Category = category },
                new Transaction(345, "", today) { Category = category },
                new Transaction(-456, "", today)
                ];
            Transaction[] transactionsOutsidePeriod = [
                new Transaction(-321, "", today.AddDays(-1)) { Category = category },
                new Transaction(-432, "", today.AddDays(1)) { Category = category },
                new Transaction(-543, "", today.AddYears(1)),
                new Transaction(654, "", today.AddMonths(5)) { Category = category }
                ];
            account.NewTransactions([.. transactionsInPeriod, .. transactionsOutsidePeriod]);

            Money expenses = -123 - 234;
            Money actualDifference = expenses + 500;

            // Act and Assert
            Assert.AreEqual(actualDifference, category.GetExpensesDifference(today, Period.Daily));
            category.ExpensesBudget = null;
            Assert.ThrowsException<CategoryException>(() => { category.GetExpensesDifference(today, Period.Daily); });
        }

        [TestMethod]
        public void GetBalanceDifference_Tests()
        {
            // - Gets the difference between the balance in a certain period, and the balance of the income and expenses budget
            // - Throws a CategoryException if ExpensesBudget or IncomeBudget is null
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var account = new Account("");
            var category = new Category("");
            category.IncomeBudget = new Budget(500, Period.Daily);
            category.ExpensesBudget = new Budget(-300, Period.Daily);

            Transaction[] transactionsInPeriod = [
                new Transaction(-123, "", today) { Category = category },
                new Transaction(-234, "", today) { Category = category },
                new Transaction(345, "", today) { Category = category },
                new Transaction(-456, "", today)
                ];
            Transaction[] transactionsOutsidePeriod = [
                new Transaction(-321, "", today.AddDays(-1)) { Category = category },
                new Transaction(-432, "", today.AddDays(1)) { Category = category },
                new Transaction(-543, "", today.AddYears(1)),
                new Transaction(654, "", today.AddMonths(5)) { Category = category }
                ];
            account.NewTransactions([.. transactionsInPeriod, .. transactionsOutsidePeriod]);

            Money balance = -123 + -234 + 345;
            Money actualDifference = balance - (500 - 300);

            // Act and Assert
            Assert.AreEqual(actualDifference, category.GetBalanceDifference(today, Period.Daily));
            category.ExpensesBudget = null;
            Assert.ThrowsException<CategoryException>(() => { category.GetBalanceDifference(today, Period.Daily); });
            category.ExpensesBudget = new Budget(0, Period.Null);
            category.IncomeBudget = null;
            Assert.ThrowsException<CategoryException>(() => { category.GetBalanceDifference(today, Period.Daily); });
        }
    }
}
