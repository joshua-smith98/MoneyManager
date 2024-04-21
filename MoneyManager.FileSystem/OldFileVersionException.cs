namespace MoneyManager.FileSystem
{
    internal class OldFileVersionException(int oldVersion) : Exception
    {
        public int OldVersion { get; } = oldVersion;
    }
}
