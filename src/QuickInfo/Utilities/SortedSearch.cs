using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickInfo
{
    public struct SortedSearch
    {
        private Func<int, string> item;
        private int count;

        public SortedSearch(Func<int, string> listItem, int listCount)
        {
            this.item = listItem;
            this.count = listCount;
        }

        public static int FindItem<T>(IList<T> list, string word, Func<T, string> keySelector)
        {
            var bounds = FindBounds(list, word, keySelector);

            if (bounds.high < bounds.low)
            {
                return -1;
            }

            return bounds.low;
        }

        private static (int low, int high) FindBounds<T>(IList<T> list, string word, Func<T, string> keySelector)
        {
            var search = new SortedSearch(i => keySelector(list[i]), list.Count);
            var bounds = search.FindBounds(word);
            return bounds;
        }

        public static IEnumerable<U> FindItems<T, U>(IList<T> list, IEnumerable<string> words, Func<T, string> keySelector, Func<T, U> itemSelector)
        {
            var hits = new HashSet<U>();
            bool firstWord = true;
            foreach (var word in words)
            {
                var bounds = FindBounds(list, word, keySelector);
                if (bounds.high < bounds.low)
                {
                    return Array.Empty<U>();
                }

                var hitsForWord = Enumerable
                    .Range(bounds.low, bounds.high - bounds.low + 1)
                    .Select(i => itemSelector(list[i]));
                if (firstWord)
                {
                    hits.UnionWith(hitsForWord);
                }
                else
                {
                    hits.IntersectWith(hitsForWord);
                }

                firstWord = false;
            }

            return hits;
        }

        public (int low, int high) FindBounds(string word)
        {
            var low = 0;
            var high = this.count - 1;
            word = word.ToUpperInvariant();

            for (int charIndex = 0; charIndex < word.Length; charIndex++)
            {
                int letterStart = FindLetterStart(low, high, word[charIndex], charIndex);
                if (letterStart == -1)
                {
                    high = low - 1;
                    break;
                }

                int letterEnd = FindLetterEnd(low, high, word[charIndex], charIndex);
                low = letterStart;
                high = letterEnd;

                if (high < low)
                {
                    break;
                }
            }

            return (low, high);
        }

        private int FindLetterStart(int low, int high, char ch, int index)
        {
            while (low < high)
            {
                int mid = low + (high - low) / 2;
                string name = this.item(mid);
                if (name.Length <= index || char.ToUpperInvariant(name[index]) < ch)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid;
                }
            }

            if (low >= this.count ||
                this.item(low).Length <= index ||
                char.ToUpperInvariant(item(low)[index]) != ch)
            {
                return -1;
            }

            return low;
        }

        private int FindLetterEnd(int low, int high, char ch, int index)
        {
            while (low < high)
            {
                int mid = low + (high - low + 1) / 2;
                string name = item(mid);
                if (name.Length <= index || char.ToUpperInvariant(item(mid)[index]) <= ch)
                {
                    low = mid;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return high;
        }

        public static (string, int)[] CreateIndex<T>(IEnumerable<T> list, Func<T, IEnumerable<string>> fieldsGetter)
        {
            var result = new List<(string, int)>(list.Count());

            int index = 0;
            foreach (var item in list)
            {
                var fields = fieldsGetter(item);
                foreach (var field in fields)
                {
                    var key = field;
                    if (!string.IsNullOrEmpty(key))
                    {
                        result.Add((key, index));
                    }
                }

                index++;
            }

            result.Sort((left, right) => StringComparer.OrdinalIgnoreCase.Compare(left.Item1, right.Item1));

            return result.ToArray();
        }
    }
}
