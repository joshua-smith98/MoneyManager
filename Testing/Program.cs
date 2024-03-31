using MoneyManager.Core;

// Construct test accounts
Account testAccount1 = new("Spending");
testAccount1.AddTransaction(new Transaction(160.92M, "[Initial Balance]"));
testAccount1.AddTransaction(new Transaction(1500, "Michael", "Wage from michael"));
testAccount1.AddTransactions(new Transaction(-200, "John", "John rent"), new Transaction(354.73M, "Tim", "Winnings"));

Account testAccount2 = new("Savings");
testAccount2.AddTransaction(new Transaction(793.10M, "[Initial Balance]"));

// Print account tables
Console.WriteLine($"{"Value",-20}   {"Payee",-20}   {"Type",-20}   {"Memo",-20}   {"Balance",-20}");
Console.WriteLine();

foreach (Transaction t in testAccount1.Transactions)
{
    Console.WriteLine($"{t.Value,20} | {t.Payee,-20} | {t.TransactionType,-20} | {t.Memo,-20} | {testAccount1.BalanceInfoAt(t).Balance,20}");
}
Console.WriteLine();
Console.WriteLine($"{"Value",-20}   {"Payee",-20}   {"Type",-20}   {"Memo",-20}   {"Balance",-20}");
Console.WriteLine();
foreach (Transaction t in testAccount2.Transactions)
{
    Console.WriteLine($"{t.Value,20} | {t.Payee,-20} | {t.TransactionType,-20} | {t.Memo,-20} | {testAccount2.BalanceInfoAt(t).Balance,20}");
}

Console.WriteLine();

// Print Income/Expenses table
Console.WriteLine($"{"Account",-20}   {"Income",-20}   {"Expenses",-20}   {"Balance",-20}");
Console.WriteLine($"{testAccount1.Name,-20} | {testAccount1.BalanceInfo.Income,20} | {testAccount1.BalanceInfo.Expenses,20} | {testAccount1.BalanceInfo.Balance,20}");
Console.WriteLine($"{testAccount2.Name,-20} | {testAccount2.BalanceInfo.Income,20} | {testAccount2.BalanceInfo.Expenses,20} | {testAccount2.BalanceInfo.Balance,20}");

Console.ReadKey();