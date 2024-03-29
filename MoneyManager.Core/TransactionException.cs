﻿namespace MoneyManager.Core
{
    /// <summary>
    /// Generic Exception for errors related to Transactions.
    /// </summary>
    public class TransactionException(string message) : Exception(message) { }
}
