using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace MoneyManager.Core
{
    /// <summary>
    /// A collection of <see cref="ReportChunkCategory"/>, with the keys being <see cref="Category"/>.
    /// A key of <see cref="null"/> gets the <see cref="ReportChunkCategory"/> representing transactions that were uncategorised.
    /// </summary>
    /// <param name="reportChunkCategories"></param>
    public class ReportChunkCategoryCollection(ReportChunkCategory[] reportChunkCategories) : IReadOnlyDictionary<Category?, ReportChunkCategory>
    {
        private ReportChunkCategory[] reportChunkCategories = reportChunkCategories;
        
        public ReportChunkCategory this[Category? key] => ContainsKey(key) ?
            reportChunkCategories.Where(x => x.Category == key).First() : throw new IndexOutOfRangeException();

        public IEnumerable<Category?> Keys => reportChunkCategories.Select(x => x.Category).Distinct();

        public IEnumerable<ReportChunkCategory> Values => reportChunkCategories;

        public int Count => reportChunkCategories.Length;

        public bool ContainsKey(Category? key) => Keys.Contains(key);

        public IEnumerator<KeyValuePair<Category?, ReportChunkCategory>> GetEnumerator()
            => reportChunkCategories.Select(x => new KeyValuePair<Category?, ReportChunkCategory>(x.Category, x)).GetEnumerator();

        public bool TryGetValue(Category? key, [MaybeNullWhen(false)] out ReportChunkCategory value)
        {
            var ret = ContainsKey(key);
            if (ret) value = this[key];
            else value = null;
            return ret;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
