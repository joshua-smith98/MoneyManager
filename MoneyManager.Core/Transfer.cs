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

        public override string Memo
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

        private Transfer(Account to, Account from, Money value, DateOnly date, string memo = "", string number = "", Transfer? twin = null)
            : base(value, date, "", memo, number)
        {
            To = to;
            From = from;
            if (twin is null)
                Twin = new Transfer(to, from, value, date, memo, number, this);
            else Twin = twin;
        }

        private Transfer(Account to, Account from, Money value, string memo = "", string number = "", Transfer? twin = null)
            : base(value, "", memo, number)
        {
            To = to;
            From = from;
            if (twin is null)
                Twin = new Transfer(to, from, value, memo, number, twin);
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
