using System.Reflection.Metadata.Ecma335;

namespace MoneyManager.Core
{
    /// <summary>
    /// A special <see cref="Transaction"/> that represents a transfer of money between two <see cref="Account"/>s.
    /// </summary>
    internal class Transfer : Transaction
    {
        /// <summary>
        /// The <see cref="Account"/> the money was transfered to.
        /// </summary>
        public Account To { get; }

        /// <summary>
        /// The <see cref="Account"/> the money was transferred from.
        /// </summary>
        public Account From { get; }

        public Transfer Twin { get; }

        public override string? Number 
        {
            get => base.Number;
            set
            {
                if (Twin.Number != value)
                    Twin.Number = value;
                base.Number = value;
            }
        }

        public override Money Value
        {
            get => base.Value;
            set
            {
                // Check for change of sign (invalid in Transfers)
                if (Math.Sign((decimal)base.Value) != Math.Sign((decimal)value))
                    throw new TransferSignChangedException("Attempted to change the sign of a Transfer. To reverse a transfer, delete this one and create new.");

                if (Twin.Value != -value)
                    Twin.Value = -value;
                base.Value = value;
            }
        }

        public override DateOnly Date
        {
            get => base.Date;
            set
            {
                if (Twin.Date != value)
                    Twin.Date = value;
                base.Date = value;
            }
        }

        public override string Payee => Value > 0 ? $"[From {From.Name}]" : $"[To {To.Name}]";

        public override string? Memo
        {
            get => base.Memo;
            set
            {
                if (Twin.Memo != value)
                    Twin.Memo = value;
                base.Memo = value;
            }
        }

        public override Category? Category
        {
            get => base.Category;
            set
            {
                if (Twin.Category != value)
                    Twin.Category = value;
                base.Category = value;
            }
        }

        public override TransactionType TransactionType => TransactionType.Transfer;

        private Transfer(Account to, Account from, Money value, DateOnly date, string memo, string number)
            : base(value, "", date, memo, number)
        {
            To = to;
            From = from;
            Twin = new Transfer(to, from, this, -value, date, memo, number);
        }

        private Transfer(Account to, Account from, Money value, DateOnly date, string memo)
            : base(value, "", date, memo)
        {
            To = to;
            From = from;
            Twin = new Transfer(to, from, this, -value, date, memo);
        }

        private Transfer(Account to, Account from, Money value, DateOnly date)
            : base(value, "", date)
        {
            To = to;
            From = from;
            Twin = new Transfer(to, from, this, -value, date);
        }

        private Transfer(Account to, Account from, Money value, string memo)
            : base(value, "", memo)
        {
            To = to;
            From = from;
            Twin = new Transfer(to, from, this, -value, memo);
        }

        private Transfer(Account to, Account from, Money value)
            : base(value, "")
        {
            To = to;
            From = from;
            Twin = new Transfer(to, from, this, -value);
        }

        private Transfer(Account to, Account from, Transfer twin, Money value, DateOnly date, string memo, string number)
            : base(value, "", date, memo, number)
        {
            To = to;
            From = from;
            Twin = twin;
        }

        private Transfer(Account to, Account from, Transfer twin, Money value, DateOnly date, string memo)
            : base(value, "", date, memo)
        {
            To = to;
            From = from;
            Twin = twin;
        }

        private Transfer(Account to, Account from, Transfer twin, Money value, DateOnly date)
            : base(value, "", date)
        {
            To = to;
            From = from;
            Twin = twin;
        }

        private Transfer(Account to, Account from, Transfer twin, Money value, string memo)
            : base(value, "", memo)
        {
            To = to;
            From = from;
            Twin = twin;
        }

        private Transfer(Account to, Account from, Transfer twin, Money value)
            : base(value, "")
        {
            To = to;
            From = from;
            Twin = twin;
        }

        public static void Create(Account from, Account to, Money value, DateOnly date, string memo, string number)
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, date, memo, number));

        public static void Create(Account from, Account to, Money value, DateOnly date, string memo)
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, date, memo));

        public static void Create(Account from, Account to, Money value, DateOnly date)
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, date));

        public static void Create(Account from, Account to, Money value, string memo)
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, memo));

        public static void Create(Account from, Account to, Money value)
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value));

        private static void CreateFrom(Transfer fromTransfer)
        {
            var from = fromTransfer.From;
            var to = fromTransfer.To;
            var toTransfer = fromTransfer.Twin;
            from.AddTransfer(fromTransfer);
            to.AddTransfer(toTransfer);
        }

        public void Delete()
        {
            if (Value > 0)
            {
                To.RemoveTransfer(this);
                From.RemoveTransfer(Twin);
                Category = null;
            }
            else
            {
                From.RemoveTransfer(this);
                To.RemoveTransfer(Twin);
                Category = null;
            }
        }

        /// <summary>
        /// Returns <see cref="true"/> if the given <see cref="Account"/> is the one money was transferred to.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool TransfersTo(Account account) => ReferenceEquals(account, To);

        /// <summary>
        /// Returns <see cref="true"/> if the given <see cref="Account"/> is the one money was transferred from.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool TransfersFrom(Account account) => ReferenceEquals(account, From);
    }
}
