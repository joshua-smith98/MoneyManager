using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoneyManager.Core
{
    /// <summary>
    /// A special <see cref="Transaction"/> that represents a transfer of money between two <see cref="Account"/>s.
    /// </summary>
    public class Transfer : Transaction
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

        public override string Number 
        {
            get => base.Number;
            set
            {
                base.Number = value;
                if (Twin is not null && Twin.Number != value)
                    Twin.Number = value;
            }
        }

        public override Money Value
        {
            get => base.Value;
            set
            {
                // Check for change of sign (invalid in Transfers)
                if (base.Value is not null && Math.Sign((decimal)base.Value) != Math.Sign((decimal)value))
                    throw new TransferSignChangedException("Attempted to change the sign of a Transfer. To reverse a transfer, delete this one and create new.");

                base.Value = value;
                if (Twin is not null && Twin.Value != -value)
                    Twin.Value = -value;
            }
        }

        public override DateOnly Date
        {
            get => base.Date;
            set
            {
                base.Date = value;
                if (Twin is not null && Twin.Date != value)
                    Twin.Date = value;
            }
        }

        public override string Payee => Value > 0 ? $"[From {From.Name}]" : $"[To {To.Name}]";

        public override string Memo
        {
            get => base.Memo;
            set
            {
                base.Memo = value;
                if (Twin is not null && Twin.Memo != value)
                    Twin.Memo = value;
            }
        }

        public override Category? Category
        {
            get => base.Category;
            set
            {
                base.Category = value;
                if (Twin is not null && Twin.Category != value)
                    Twin.Category = value;
            }
        }

        public override TransactionType TransactionType => TransactionType.Transfer;

        private Transfer(Account to, Account from, Money value, DateOnly date, string memo = "", string number = "", Transfer? twin = null)
            : base(value, date, "", memo, number)
        {
            To = to;
            From = from;
            if (twin is null)
                Twin = new Transfer(to, from, -value, date, memo, number, this);
            else Twin = twin;
        }

        private Transfer(Account to, Account from, Money value, string memo = "", string number = "", Transfer? twin = null)
            : base(value, "", memo, number)
        {
            To = to;
            From = from;
            if (twin is null)
                Twin = new Transfer(to, from, -value, memo, number, this);
            else Twin = twin;
        }

        public static void Create(Account from, Account to, Money value, DateOnly date, string memo = "", string number = "")
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, date, memo, number));

        public static void Create(Account from, Account to, Money value, string memo = "", string number = "")
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, memo, number));

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
            }
            else
            {
                From.RemoveTransfer(this);
                To.RemoveTransfer(Twin);
            }
            Category = null;
        }
    }
}
