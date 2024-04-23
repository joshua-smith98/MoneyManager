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

        /// <summary>
        /// This <see cref="Transfer"/>'s twin in the second <see cref="Account"/>.
        /// </summary>
        public Transfer Twin { get; }

        /// <summary>
        /// Gets whether this <see cref="Transfer"/> is incoming or outgoing.
        /// </summary>
        public TransferType TransferType => Value switch
        {
            var v when v > 0 => TransferType.Incoming,
            var v when v < 0 => TransferType.Outgoing,
            _ => TransferType.Null
        };

        public override string Number 
        {
            get => base.Number;
            set
            {
                // Set the twin's value as well
                base.Number = value;
                if (Twin is not null && Twin.Number != value) // We need the null check here to avoid a NullReferenceException upon initialisation.
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
                    throw new TransferException("Attempted to change the sign of a Transfer. To reverse a transfer, delete this one and create new.");

                // Set the twin's value as well
                base.Value = value;
                if (Twin is not null && Twin.Value != -value) // See: Number
                    Twin.Value = -value;
            }
        }

        public override DateOnly Date
        {
            get => base.Date;
            set
            {
                // Set the twin's value as well
                base.Date = value;
                if (Twin is not null && Twin.Date != value) // See: Number
                    Twin.Date = value;
            }
        }

        // Automatically set the Payee property based on whether this Transfer is incoming or outgoing
        public override string Payee => TransferType is TransferType.Incoming ? $"[From {From.Name}]" : $"[To {To.Name}]";

        public override string Memo
        {
            get => base.Memo;
            set
            {
                // Set the twin's value as well
                base.Memo = value;
                if (Twin is not null && Twin.Memo != value) // See: Number
                    Twin.Memo = value;
            }
        }

        public override Category? Category
        {
            get => base.Category;
            set
            {
                // Set the twin's value as well
                base.Category = value;
                if (Twin is not null && Twin.Category != value) // See: Number
                    Twin.Category = value;
            }
        }

        // TransactionType is always Transfer for a Transfer
        public override TransactionType TransactionType => TransactionType.Transfer;

        /* The Transfer construction logic is a bit complex, so here's a description:
         *  First    -> The Create() method is called. This constructs the outgoing transfer, without providing a twin.
         *  Then     -> The outgoing transfer's constructor constructs the incoming transfer, giving itself as its twin,
         *              and assigning the incoming transfer as the outgoing's twin. The two transfers are now linked.
         *  Finally  -> The outgoing transfer is passed to the CreateFrom() method, which adds both transfers to their respective Accounts.
         */

        private Transfer(Account to, Account from, Money value, DateOnly date, string memo = "", string number = "", Category? category = null, Transfer? twin = null)
            : base(value, date, "", memo, number, category)
        {
            To = to;
            From = from;

            // Case: twin isn't given -> create our own twin with inverse value
            if (twin is null)
                Twin = new Transfer(to, from, -value, date, memo, number, category, this);
            else Twin = twin; // Otherwise just use the one we're given
        }

        private Transfer(Account to, Account from, Money value, string memo = "", string number = "", Category? category = null, Transfer? twin = null)
            : base(value, "", memo, number, category)
        {
            To = to;
            From = from;

            // Case: twin isn't given -> create our own twin with inverse value
            if (twin is null)
                Twin = new Transfer(to, from, -value, memo, number, category, this);
            else Twin = twin; // Otherwise just use the one we're given
        }

        /// <summary>
        /// Creates a new transfer between the given accounts, with the given metadata.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="value"></param>
        /// <param name="date"></param>
        /// <param name="memo"></param>
        /// <param name="number"></param>
        /// <param name="category"></param>
        public static void Create(Account from, Account to, Money value, DateOnly date, string memo = "", string number = "", Category? category = null)
            // Note: we are constructing the outgoing transfer, so we're ensuring value is negative here.
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, date, memo, number, category));

        /// <summary>
        /// Creates a new transfer between the given accounts, with the given metadata.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="value"></param>
        /// <param name="memo"></param>
        /// <param name="number"></param>
        /// <param name="category"></param>
        public static void Create(Account from, Account to, Money value, string memo = "", string number = "", Category? category = null)
            // Note: we are constructing the outgoing transfer, so we're ensuring value is negative here.
            => CreateFrom(new Transfer(to, from, value > 0 ? -value : value, memo, number, category));

        private static void CreateFrom(Transfer fromTransfer)
        {
            // Aquire accounts and the twin transfer
            var from = fromTransfer.From;
            var to = fromTransfer.To;
            var toTransfer = fromTransfer.Twin;

            // Add transfers to accounts
            from.AddTransfer(fromTransfer);
            to.AddTransfer(toTransfer);
        }

        /// <summary>
        /// Removes this <see cref="Transfer"/> and its twin from their accounts, and detaches them from their categories.
        /// </summary>
        public void Delete()
        {
            // Case: this is the incoming transfer
            if (TransferType is TransferType.Incoming)
            {
                To.RemoveTransfer(this);
                From.RemoveTransfer(Twin);
            }
            // Case: this is the outgoing transfer
            else
            {
                From.RemoveTransfer(this);
                To.RemoveTransfer(Twin);
            }

            // Detach categories
            Category = null;
        }
    }
}
