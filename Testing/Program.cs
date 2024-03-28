using MoneyManager.Core;

TransactionCollection tc = new(0) {
    new Transaction(10, "Michael", "Wage from michael"),
    new Transaction(-35, "John", "John rent"),
    new Transaction(500, "Tim", "Winnings")
};

foreach (Transaction t in tc)
{
    Console.WriteLine($"{t.Value}\t\t{t.Payee}\t\t{t.TransactionType}\t\t{tc.BalanceAt(t)}\t\t{t.Memo}");
}
Console.ReadKey();