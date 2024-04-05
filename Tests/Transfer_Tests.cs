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
            var account1 = new Account("");
            var account2 = new Account("");
            account1.NewTransaction(new Transaction(500, "[Initial Balance]"));

            // Create and retrieve transfer
            account1.TransferTo(account2, 200);
            Transfer transfer1 = (Transfer)account1.Transactions[^1];
            Transfer transfer2 = (Transfer)account2.Transactions[^1];

            // Act
            transfer1.Number = "12345";

            // Assert
        }
    }
}
