namespace MoneyManager.Core
{
    /// <summary>
    /// Represents any <see cref="Exception"/> relating to the <see cref="Budget"/> class.
    /// </summary>
    /// <param name="message"></param>
    public class BudgetException(string message) : Exception(message) { }
}
