using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Budget_Tests
    {
        [TestMethod]
        public void Sum_Test()
        {
            // Gets a new budget the is the sum of the two given budgets
            // Arrange
            Money value1 = 250;
            Money value2 = 500;

            var budget1 = new Budget(value1, Period.Null);
            var budget2 = new Budget(value2, Period.Null);

            var actualSum = value1 + value2;

            // Act
            var budgetSum = Budget.Sum(budget1, budget2, Period.Null);

            // Act and Assert
            Assert.AreEqual(budgetSum.Get(Period.Null), actualSum);
        }

        [TestMethod]
        public void GetSet_Test()
        {
            // Set():
            //  - Sets the value of the budget to the given value for the given period
            //  - Sets the budget's CurrentPeriod to the given period
            // Get():
            //  - Gets the value of the budget for the given period
            // Both throw an exception if the period is not valid (null?)

            // Arrange
            Money value = 55;
            var budget = new Budget(0, Period.Null);
            int invalidEnum = -50;

            // Act and Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => budget.Set(0, (Period)invalidEnum)); // Test invalid enums
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => budget.Get((Period)invalidEnum));

            foreach (Period period in Enum.GetValues(typeof(Period)))
            {
                budget.Set(value, period);
                var result = budget.Get(period);
                Assert.AreEqual(result, value); // Check value is correctly stored and retrieved
                Assert.AreEqual(period, budget.CurrentPeriod); // Check CurrentPeriod is correctly stored

                // We don't need to check all permutations of Period since the class always converts to Daily and back - so if there's a problem we'll see it here.
            }
        }
    }
}
