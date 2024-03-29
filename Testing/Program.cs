using MoneyManager.Core;

Account testAccount1 = new("Spending", 0);
testAccount1.AddTransaction(new Transaction(1500, "Michael", "Wage from michael"));
testAccount1.AddTransactions(new Transaction(-200, "John", "John rent"), new Transaction(354.73M, "Tim", "Winnings"));

Account testAccount2 = new("Savings", 1);
testAccount1.TransferTo(1000, testAccount2, "Savings increment");
testAccount1.TransferFrom(200, testAccount2, "Savings correction");

Console.WriteLine($"{"Value",-20}   {"Payee",-20}   {"Type",-20}   {"Memo",-20}   {"Balance",-20}");
Console.WriteLine();

foreach (Transaction t in testAccount1.Transactions)
{
    Console.WriteLine($"{t.Value,20} | {t.Payee,-20} | {t.TransactionType,-20} | {t.Memo,-20} | {testAccount1.BalanceAt(t),-20}");
}
Console.WriteLine();
Console.WriteLine($"{"Value",-20}   {"Payee",-20}   {"Type",-20}   {"Memo",-20}   {"Balance",-20}");
Console.WriteLine();
foreach (Transaction t in testAccount2.Transactions)
{
    Console.WriteLine($"{t.Value,20} | {t.Payee,-20} | {t.TransactionType,-20} | {t.Memo,-20} | {testAccount1.BalanceAt(t),-20}");
}

Console.ReadKey();