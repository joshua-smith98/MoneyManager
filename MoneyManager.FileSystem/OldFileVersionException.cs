namespace MoneyManager.FileSystem
{
    /// <summary>
    /// Thrown when a file uses a version that is out of date or otherwise invalid.
    /// </summary>
    /// <param name="oldVersion">The version of the out of date file.</param>
    internal class OldFileVersionException(int oldVersion) : Exception
    {
        /// <summary>
        /// The version of the out of date file.
        /// </summary>
        public int OldVersion { get; } = oldVersion;
    }
}
