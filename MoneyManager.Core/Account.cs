using System.Transactions;

namespace MoneyManager.Core
{
    public class Account
    {
        public string Name { get; set; }
        public MoneyValue Balance => BalanceAt(transactions.Last());
        private readonly MoneyValue initialBalance;
        public Transaction[] Transactions => transactions.ToArray();
        private readonly List<Transaction> transactions = [];

        /// <summary>
        /// Gets the number of <see cref="Transaction"/>s contained within this <see cref="Account"/>.
        /// </summary>
        public int Count => transactions.Count;

        public Account(string name, MoneyValue initialBalance)
        {
            Name = name;
            this.initialBalance = initialBalance;
        }

        public Account(string name, MoneyValue initialBalance, params Transaction[] transactions)
        {
            Name = name;
            this.initialBalance = initialBalance;
            this.transactions.AddRange(transactions);
        }

        /// <summary>
        /// Gets the balance of this <see cref="Account"/> at the given <see cref="Transaction"/>.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public MoneyValue BalanceAt(Transaction transaction)
        {
            // Validity check: transaction must be in this collection
            if (!transactions.Contains(transaction)) throw new IndexOutOfRangeException();

            return BalanceAtIndex(transactions.IndexOf(transaction));
        }

        /// <summary>
        /// Gets the balance of this <see cref="Account"/> at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public MoneyValue BalanceAtIndex(int index)
        {
            // Validity check: index must be within the bounds of this collection
            if (index >= transactions.Count) throw new IndexOutOfRangeException();

            MoneyValue total = initialBalance;

            for (int i = 0; i <= index; i++)
            {
                if (transactions[i].IsCleared)
                {
                    if (transactions[i] is Transfer t) total += t.TransfersTo(this) ? t.Value : -(t.Value); // Value is positive if transfer is to, negative is transfer if from
                    else total += transactions[i].Value;
                }
            }

            return total;
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction is Transfer) throw new TransactionInvalidException();
            if (transactions.Contains(transaction)) throw new TransactionAlreadyExistsException();
            transactions.Add(transaction);
        }

        public void AddTransactions(params Transaction[] transactions)
            => this.transactions.AddRange(transactions);

        public void RemoveTransaction(Transaction transaction)
        {
            // Validity check: transaction must be in this account
            if (!transactions.Contains(transaction)) throw new IndexOutOfRangeException();
            
            // Transfer removal logic
            if (transaction is Transfer t)
            {
                var twin = t.TransfersFrom(this) ? t.To : t.From;
                transactions.Remove(t);
                if (!twin.transactions.Remove(t))
                    throw new Exception("Transfer is not in both Accounts as it should be. This should never be the case!");
            }
            else transactions.Remove(transaction);
        }

        public void RemoveTransactionAt(int index)
        {
            // Validity check: index must be in range
            if (index >= transactions.Count) throw new IndexOutOfRangeException();
            transactions.RemoveAt(index);
        }

        private void AddTransfer(Transfer transfer, Account twin)
        {
            transactions.Add(transfer);
            twin.transactions.Add(transfer);
        }

        public void TransferTo(Account account, MoneyValue value)
        {
            var transfer = new Transfer(account, this, value, Name);
            AddTransfer(transfer, account);
        }

        public void TransferTo(Account account, MoneyValue value, string memo)
        {
            var transfer = new Transfer(account, this, value, Name, memo);
            AddTransfer(transfer, account);
        }

        public void TransferTo(Account account, MoneyValue value, DateOnly date)
        {
            var transfer = new Transfer(account, this, value, Name, date);
            AddTransfer(transfer, account);
        }

        public void TransferTo(Account account, MoneyValue value, DateOnly date, string memo)
        {
            var transfer = new Transfer(account, this, value, Name, date, memo);
            AddTransfer(transfer, account);
        }

        public void TransferTo(Account account, MoneyValue value, DateOnly date, string memo, string transactionNumber)
        {
            var transfer = new Transfer(account, this, value, Name, date, memo, transactionNumber);
            AddTransfer(transfer, account);
        }

        public void TransferFrom(Account account, MoneyValue value)
            => account.TransferTo(this, value);

        public void TransferFrom(Account account, MoneyValue value, string memo)
            => account.TransferTo(this, value, memo);

        public void TransferFrom(Account account, MoneyValue value, DateOnly date)
            => account.TransferTo(this, value, date);

        public void TransferFrom(Account account, MoneyValue value, DateOnly date, string memo)
            => account.TransferTo(this, value, date, memo);

        public void TransferFrom(Account account, MoneyValue value, DateOnly date, string memo, string transactionNumber)
            => account.TransferTo(this, value, date, memo, transactionNumber);
    }
}
