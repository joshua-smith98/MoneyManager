using MoneyManager.Core;

namespace Tests
{
    [TestClass]
    public class Money_Tests
    {
        [TestMethod]
        public void Equals_Tests()
        {
            // Arrange
            Money money1 = 5.34m;
            Money money2 = 5.34m;
            Money money3 = 2;

            // Assert
            Assert.AreEqual(money1, money1);
            Assert.AreEqual(money1, money2);
            Assert.AreNotEqual(money1, money3);
        }

        [TestMethod]
        public void Comparison_Tests()
        {
            // Arrange
            Money money1 = 5;
            Money money2 = 7;
            Money money3 = 5;

            // Assert
            Assert.IsTrue(money1 < money2); // Less than
            Assert.IsTrue(money2 > money1); // Greater than
            Assert.IsTrue(money1 <= money2); // >Less than< or equal to
            Assert.IsTrue(money2 >= money1); // >Greater than< or equal to
            Assert.IsTrue(money1 <= money3); // Less than or >equal to<
            Assert.IsTrue(money1 >= money3); // Greater than or >equal to<
            Assert.IsFalse(money1 < money3); // equal is not less than
            Assert.IsFalse(money1 > money3); // equal is not greater than
        }

        [TestMethod]
        public void Arithmetic_Tests()
        {
            // Arrange
            Money money1 = 16;

            // Act
            var positive = +money1;
            var negative = -money1;
            var addition = money1 + 10;
            var subtractition = money1 - 10;
            var multiplication = money1 * 4;
            var division = money1 / 4;

            // Assert
            Assert.AreEqual(positive, 16);
            Assert.AreEqual(negative, -16);
            Assert.AreEqual(addition, 16 + 10);
            Assert.AreEqual(subtractition, 16 - 10);
            Assert.AreEqual(multiplication, 16 * 4);
            Assert.AreEqual(division, 16 / 4);
        }

        [TestMethod]
        public void ToString_Tests()
        {
            // Arrange
            decimal value = 2394875.3487m;
            Money money1 = value;

            // Act
            var decimalStr = value.ToString("C");
            var moneyString = money1.ToString();

            // Assert
            Assert.AreEqual(decimalStr, moneyString);
        }
    }
}