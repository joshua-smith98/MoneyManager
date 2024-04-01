namespace MoneyManager.Core
{
    /// <summary>
    /// Represents any <see cref="Exception"/> related to the <see cref="Category"/> class.
    /// </summary>
    /// <param name="message"></param>
    public class CategoryException(string message) : Exception(message) { }
}
