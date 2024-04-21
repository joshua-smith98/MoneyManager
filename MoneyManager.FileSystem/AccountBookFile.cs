using MoneyManager.Core;

namespace MoneyManager.FileSystem
{
    public class AccountBookFile : IFile<AccountBook, AccountBookFile>
    {
        public static char[] Header => "MOMAACBK".ToArray();

        public static string Extension => ".accbk";

        public static int Version => 1;

        public string? Path { get; private set; }

        private AccountBookFile(string? path)
            => Path = path;

        public static AccountBookFile LoadFrom(string path)
        {
            throw new NotImplementedException();
        }

        public static AccountBookFile Deconstruct(AccountBook accountBook)
        {
            throw new NotImplementedException();
        }

        public void UpdateFrom(AccountBook accountBook)
        {
            throw new NotImplementedException();
        }

        public void SaveTo(string path)
        {
            throw new NotImplementedException();
        }

        public AccountBook Construct()
        {
            throw new NotImplementedException();
        }

    }
}
