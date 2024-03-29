namespace MoneyManager.Core
{
    /// <summary>
    /// Represents an account of monetary transactions.
    /// </summary>
    public class Account
    {
        public string Name { get; set; }
        
        /// <summary>
        /// The balance of all cleared <see cref="Transaction"/>s contained within this <see cref="Account"/>.
        /// </summary>
        public MoneyValue Balance => BalanceAt(transactions.Last());
        private readonly MoneyValue initialBalance;
        
        /// <summary>
        /// The collection of <see cref="Transaction"/>s contained within this <see cref="Account"/>.
        /// </summary>
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

            // Calculate balance
            MoneyValue total = initialBalance;

            for (int i = 0; i <= index; i++)
            {
                // Only count transactions that are cleared
                if (transactions[i].IsCleared)
                {
                    // Special case for Transfers: Their balance is positive if transfer is to this account, negative if transfer is from
                    if (transactions[i] is Transfer t) total += t.TransfersTo(this) ? t.Value : -(t.Value);
                    // General transaction case
                    else total += transactions[i].Value;
                }
            }

            return total;
        }

        /// <summary>
        /// Adds the given <see cref="Transaction"/> to this <see cref="Account"/>.
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="TransactionInvalidException"></exception>
        /// <exception cref="TransactionAlreadyExistsException"></exception>
        public void AddTransaction(Transaction transaction)
        {
            // Validity check: Can't add a Transfer using this method
            if (transaction is Transfer) throw new TransactionInvalidException("Tried to add a Transfer as if it were a Transaction.");

            // Validity check: Transaction can't already be in this account
            if (transactions.Contains(transaction)) throw new TransactionAlreadyExistsException($"Tried to add duplicate transaction to account: \"{Name}\"");
            
            transactions.Add(transaction);
        }

        /// <summary>
        /// Adds the given collection of <see cref="Transaction"/>s to this <see cref="Account"/>.
        /// </summary>
        /// <param name="transactions"></param>
        public void AddTransactions(params Transaction[] transactions)
            => this.transactions.AddRange(transactions);

        /// <summary>
        /// Removed the given <see cref="Transaction"/> from this <see cref="Account"/>
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public void RemoveTransaction(Transaction transaction)
        {
            // Validity check: transaction must be in this account
            if (!transactions.Contains(transaction)) throw new IndexOutOfRangeException();
            
            // Case: this Transaction is a Transfer
            if (transaction is Transfer t)
            {
                // Get the Transfer's twin account, and remove it from both this and that
                var twin = t.TransfersFrom(this) ? t.To : t.From;
                transactions.Remove(t);
                if (!twin.transactions.Remove(t))
                    throw new Exception("Transfer is not in both Accounts as it should be. This should never be the case!");
            }
            else transactions.Remove(transaction);
        }

        /// <summary>
        /// Removes the <see cref="Transaction"/> at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void RemoveTransactionAt(int index)
        {
            // Validity check: index must be in range
            if (index >= transactions.Count) throw new IndexOutOfRangeException();

            transactions.RemoveAt(index);
        }

        private void AddTransfer(Transfer transfer, Account twin)
        {
            // Validity check: transfers cannot be between the same account
            if (ReferenceEquals(this, twin)) throw new TransactionInvalidException("Tried to transfer money from an account to itself.");
            
            // Add to both this account, and the twin account
            transactions.Add(transfer);
            twin.transactions.Add(transfer);
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        public void TransferTo(Account account, MoneyValue value)
        {
            var transfer = new Transfer(account, this, value, Name);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="memo"></param>
        public void TransferTo(Account account, MoneyValue value, string memo)
        {
            var transfer = new Transfer(account, this, value, Name, memo);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        public void TransferTo(Account account, MoneyValue value, DateOnly date)
        {
            var transfer = new Transfer(account, this, value, Name, date);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        public void TransferTo(Account account, MoneyValue value, DateOnly date, string memo)
        {
            var transfer = new Transfer(account, this, value, Name, date, memo);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        /// <param name="transactionNumber"></param>
        public void TransferTo(Account account, MoneyValue value, DateOnly date, string memo, string transactionNumber)
        {
            var transfer = new Transfer(account, this, value, Name, date, memo, transactionNumber);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        public void TransferFrom(Account account, MoneyValue value)
            => account.TransferTo(this, value);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="memo"></param>
        public void TransferFrom(Account account, MoneyValue value, string memo)
            => account.TransferTo(this, value, memo);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        public void TransferFrom(Account account, MoneyValue value, DateOnly date)
            => account.TransferTo(this, value, date);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        public void TransferFrom(Account account, MoneyValue value, DateOnly date, string memo)
            => account.TransferTo(this, value, date, memo);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        /// <param name="transactionNumber"></param>
        public void TransferFrom(Account account, MoneyValue value, DateOnly date, string memo, string transactionNumber)
            => account.TransferTo(this, value, date, memo, transactionNumber);
    }
}
