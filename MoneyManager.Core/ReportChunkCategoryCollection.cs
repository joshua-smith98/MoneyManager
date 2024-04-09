using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Core
{
    public class ReportChunkCategoryCollection(ReportChunkCategory[] reportChunkCategories) : IReadOnlyDictionary<Category?, ReportChunkCategory>
    {
        private ReportChunkCategory[] reportChunkCategories = reportChunkCategories;
        
        public ReportChunkCategory this[Category? key] => ContainsKey(key) ?
            reportChunkCategories.Where(x => x.Category == key).First() : throw new IndexOutOfRangeException();

        public IEnumerable<Category?> Keys => reportChunkCategories.Select(x => x.Category);

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
