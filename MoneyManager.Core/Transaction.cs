﻿namespace MoneyManager.Core
{
    /// <summary>
    /// Represents a transaction placed on a certain date, with a monetary value and various metadata.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Optional identifier for this <see cref="Transaction"/>.
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Monetary value of this <see cref="Transaction"/>.
        /// </summary>
        public virtual Money Value
        {
            // Expose the private field here so we can do a check for Null transactions
            get => _value;
            set => _value = value != 0 ? value : throw new TransactionException("Cannot set a Transaction's value to zero!");
        }
        private Money _value;

        /// <summary>
        /// Cleared monetary value of this <see cref="Transaction"/>. Gets $0.00 if <see cref="IsCleared"/> is false.
        /// </summary>
        public Money ClearedValue => IsCleared ? Value : 0;

        /// <summary>
        /// The date this <see cref="Transaction"/> was placed on.
        /// </summary>
        public virtual DateOnly Date { get; set; }

        /// <summary>
        /// The person or entity this <see cref="Transaction"/> was paid to, or by.
        /// </summary>
        public virtual string Payee { get; set; }

        /// <summary>
        /// A short description of this <see cref="Transaction"/>.
        /// </summary>
        public virtual string Memo { get; set; }

        /// <summary>
        /// The <see cref="Core.Category"/> this <see cref="Transaction"/> falls under in budgets and reports.
        /// </summary>
        public virtual Category? Category
        {
            get => category;
            set
            {
                // Remove transaction from old category and add to new
                category?.RemoveTransaction(this);
                value?.AddTransaction(this);
                category = value;
            }
        }
        private Category? category;

        /// <summary>
        /// Whether this <see cref="Transaction"/>'s payment is complete. <see cref="true"/> by default.
        /// </summary>
        public bool IsCleared { get; set; } = true;

        /// <summary>
        /// Gets the type of this <see cref="Transaction"/> (Withdrawal, Deposit, etc.).
        /// </summary>
        public virtual TransactionType TransactionType => Value switch
        {
            var v when v > 0 => TransactionType.Deposit, // Value > 0 is a Deposit
            var v when v < 0 => TransactionType.Withdrawal, // Value < 0 is a Withdrawal
            _ => TransactionType.Null
        };

        public Transaction(Money value, DateOnly date, string payee = "", string memo = "", string number = "", Category? category = null)
        {
            // Validity check: Transaction Value must not be zero
            if (value == 0) throw new TransactionException("Cannot create a Transaction with a value of zero!");
            _value = value;
            Date = date;
            Payee = payee;
            Memo = memo;
            Number = number;
            Category = category;
        }

        public Transaction(Money value, string payee = "", string memo = "", string number = "", Category? category = null)
            : this(value, DateOnly.FromDateTime(DateTime.Now), payee, memo, number, category) { }
    }
}
