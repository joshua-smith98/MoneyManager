namespace MoneyManager.FileSystem
{
    public interface IFile<T, This>
    {
        public static abstract char[] Header { get; }
        public static abstract string Extension { get; }
        public static abstract int Version { get; }

        public string? Path { get; }

        public static abstract This LoadFrom(string path);
        public static abstract This Deconstruct(T t);

        public void UpdateFrom(T t);
        public void SaveTo(string path);
        public T Construct();
    }
}
