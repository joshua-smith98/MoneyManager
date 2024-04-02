using System.Transactions;

namespace MoneyManager.Core
{
    /// <summary>
    /// Represents an account of monetary transactions.
    /// </summary>
    public class Account : Balanceable
    {
        public string Name { get; set; }

        /// <summary>
        /// The collection of <see cref="Transaction"/>s contained within this <see cref="Account"/>.
        /// </summary>
        public override Transaction[] Transactions => transactions.ToArray();
        private readonly List<Transaction> transactions = [];

        public Account(string name)
        {
            Name = name;
        }

        public Account(string name, params Transaction[] transactions)
        {
            Name = name;
            this.transactions.AddRange(transactions);
            this.transactions.Sort((x, y) => x.Date.CompareTo(y)); // Sort ascending by date at every change to transactions list
        }

        /// <summary>
        /// Adds the given <see cref="Transaction"/> to this <see cref="Account"/>.
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="TransactionInvalidException"></exception>
        /// <exception cref="TransactionAlreadyExistsException"></exception>
        public void NewTransaction(Transaction transaction)
        {
            // Validity check: Can't add a Transfer using this method
            if (transaction is Transfer) throw new TransactionInvalidException("Tried to add a Transfer as if it were a Transaction.");

            // Validity check: Transaction can't already be in this account
            if (transactions.Contains(transaction)) throw new TransactionAlreadyExistsException($"Tried to add duplicate transaction to account: \"{Name}\"");
            
            transactions.Add(transaction);
            transactions.Sort((x, y) => x.Date.CompareTo(y.Date)); // Sort ascending by date at every change to transactions list
        }

        /// <summary>
        /// Adds the given collection of <see cref="Transaction"/>s to this <see cref="Account"/>.
        /// </summary>
        /// <param name="transactions"></param>
        public void NewTransactions(params Transaction[] transactions)
        {
            // Validity check: Can't add any Transfers using this method
            if (transactions.Any(x => x is Transfer)) throw new TransactionInvalidException("Tried to add a Transfer as if it were a Transaction.");

            // Validity check: Transactions can't already be in this account
            if (transactions.Any(x => this.transactions.Contains(x))) throw new TransactionAlreadyExistsException($"Tried to add duplicate transaction to account: \"{Name}\"");

            this.transactions.AddRange(transactions);
            this.transactions.Sort((x, y) => x.Date.CompareTo(y.Date)); // Sort ascending by date at every change to transactions list
        }

        /// <summary>
        /// Deletes the given <see cref="Transaction"/> from this <see cref="Account"/>
        /// </summary>
        /// <param name="transaction"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <exception cref="Exception"></exception>
        public void DeleteTransaction(Transaction transaction)
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
            else
            {
                transactions.Remove(transaction);
                transaction.Category = null; // Remove this transaction from its category
            }
            // No need to sort here, as removing an element doesn't change the order of elements
        }

        /// <summary>
        /// Deletes the <see cref="Transaction"/> at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public void DeleteTransactionAt(int index)
        {
            // Validity check: index must be in range
            if (index >= transactions.Count) throw new IndexOutOfRangeException();

            DeleteTransaction(transactions[index]); // To avoid duplicate code
        }

        private void AddTransfer(Transfer transfer, Account twin)
        {
            // Validity check: transfers cannot be between the same account
            if (ReferenceEquals(this, twin)) throw new TransactionInvalidException("Tried to transfer money from an account to itself.");
            
            // Add to both this account, and the twin account
            transactions.Add(transfer);
            transactions.Sort((x, y) => x.Date.CompareTo(y.Date)); // Sort ascending by date at every change to transactions list
            twin.transactions.Add(transfer);
            twin.transactions.Sort((x, y) => x.Date.CompareTo(y.Date)); // Sort ascending by date at every change to transactions list (including this twin!)
        }

        /// <summary>
        /// Adds a Transfer of money from here to the given <see cref="Account"/>.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        public void TransferTo(Account account, Money value)
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
        public void TransferTo(Account account, Money value, string memo)
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
        public void TransferTo(Account account, Money value, DateOnly date)
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
        public void TransferTo(Account account, Money value, DateOnly date, string memo)
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
        public void TransferTo(Account account, Money value, DateOnly date, string memo, string transactionNumber)
        {
            var transfer = new Transfer(account, this, value, Name, date, memo, transactionNumber);
            AddTransfer(transfer, account);
        }

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        public void TransferFrom(Account account, Money value)
            => account.TransferTo(this, value);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="memo"></param>
        public void TransferFrom(Account account, Money value, string memo)
            => account.TransferTo(this, value, memo);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        public void TransferFrom(Account account, Money value, DateOnly date)
            => account.TransferTo(this, value, date);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        public void TransferFrom(Account account, Money value, DateOnly date, string memo)
            => account.TransferTo(this, value, date, memo);

        /// <summary>
        /// Adds a Transfer of money from the given <see cref="Account"/> to here.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        /// <param name="transactionNumber"></param>
        public void TransferFrom(Account account, Money value, DateOnly date, string memo, string transactionNumber)
            => account.TransferTo(this, value, date, memo, transactionNumber);
    }
}
