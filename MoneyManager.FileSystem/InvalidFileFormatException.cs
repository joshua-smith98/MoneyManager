namespace MoneyManager.FileSystem
{
    /// <summary>
    /// Represents any error resulting from an invalid file format or contents.
    /// </summary>
    /// <param name="message"></param>
    public class InvalidFileFormatException(string message) : Exception(message);
}
