namespace MoneyManager.FileSystem
{    
    /// <summary>
    /// Generic interface, representing any file format that is transmutable from and/or to a certain class.
    /// </summary>
    /// <typeparam name="T">The type of the class we are transmuting to/from.</typeparam>
    /// <typeparam name="This">This class. Always set to the type of the implementation we are working with.</typeparam>
    public interface IFile<T, This> where T : class where This : IFile<T, This>
    {
        /// <summary>
        /// The char header for this file format, placed at the beginning of the file. Usually a brief descriptor of the file format no longer than 8 bytes.
        /// </summary>
        public static abstract char[] Header { get; }
        /// <summary>
        /// The filename extension for this file format.
        /// </summary>
        public static abstract string Extension { get; }
        /// <summary>
        /// The version of this file format. Used for when changes in serialisation code breaks the read/write of an old file.
        /// </summary>
        public static abstract int Version { get; }

        /// <summary>
        /// The current path to the file, if it has been loaded or saved previously.
        /// </summary>
        public string? Path { get; }

        /// <summary>
        /// Builds a new instance of <see cref="This"/> from the file at the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static abstract This LoadFrom(string path);
        /// <summary>
        /// Deconstructs the given object into an instance of <see cref="This"/>.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static abstract This Deconstruct(T t);

        /// <summary>
        /// Updates this instance with new information from the given object.
        /// </summary>
        /// <param name="t"></param>
        public void UpdateFrom(T t);
        /// <summary>
        /// Saves this instance to a file at the given path, overwriting any existing data.
        /// </summary>
        /// <param name="path"></param>
        public void SaveTo(string path);
        /// <summary>
        /// Constructs an instance of <see cref="T"/> using this current <see cref="IFile"/> instance.
        /// </summary>
        /// <returns></returns>
        public T Construct();
    }
}
