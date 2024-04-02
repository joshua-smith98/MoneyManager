using System.Security.Principal;

namespace MoneyManager.Core
{
    public class Sheet
    {
        public Account[] Accounts => accounts.ToArray();
        private List<Account> accounts = [];

        public Category[] Categories => categories.ToArray();
        private List<Category> categories = [];

        public Sheet() { }

        public void NewAccount(Account account)
        {
            // Validity check: Accounts must not already contain account
            if (Accounts.Contains(account)) throw new SheetException($"Account \"{account.Name}\" already exists in sheet.");

            accounts.Add(account);
        }
        public void NewAccounts(params Account[] accounts)
        {
            // Validity check: Accounts must not already contain any of accounts
            foreach (Account account in accounts)
                if (Accounts.Contains(account)) throw new SheetException($"Account \"{account.Name}\" already exists in sheet.");

            this.accounts.AddRange(accounts);
        }
        public void DeleteAccount(Account account)
        {
            // Validity check: account must be within Accounts
            if (!Accounts.Contains(account)) throw new IndexOutOfRangeException();

            accounts.Remove(account);
        }
        public void DeleteAccountAt(int index)
        {
            // Validity check: index must be within the range of accounts
            if (index >= accounts.Count) throw new IndexOutOfRangeException();

            accounts.RemoveAt(index);
        }

        public void NewCategory(Category category)
        {
            // Validity check: category must not already be in Categories
            if (Categories.Contains(category)) throw new SheetException($"Category \"{category.Name}\" already exists in sheet.");

            categories.Add(category);
        }
        public void NewCategories(params Category[] categories)
        {
            // Validity check: Categories must not already contain any of categories
            foreach (Category category in categories)
            if (Categories.Contains(category)) throw new SheetException($"Category \"{category.Name}\" already exists in sheet.");

            this.categories.AddRange(categories);
        }
        public void DeleteCategory(Category category)
        {
            // Validity check: category must be contained within Categories
            if (!Categories.Contains(category)) throw new IndexOutOfRangeException();

            categories.Remove(category);

            // Remove all references to this category from all Transactions
            // PURGE IT FROM ALL SPACETIME!!
            foreach (Account account in accounts)
            {
                foreach (Transaction transaction in account.Transactions)
                {
                    if (transaction.Category == category) transaction.Category = null;
                }
            }
        }

        public void DeleteCategoryAt(int index)
        {
            // Validity check: index must be within the bounds of categories
            if (index >= categories.Count) throw new IndexOutOfRangeException();

            DeleteCategory(categories[index]); // Avoid duplicating purging code
        }
    }
}
