using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickInfo
{
    public class SeparatedList : IStructureParser
    {
        public IReadOnlyList<object> Parts { get; }
        public char SeparatorChar { get; }

        private char[] separatorCharArray;

        public SeparatedList(char separator)
        {
            SeparatorChar = separator;
            separatorCharArray = new[] { separator };
        }

        public SeparatedList(IEnumerable<object> parts, char separatorChar)
        {
            Parts = parts.ToArray();
            SeparatorChar = separatorChar;
        }

        public object this[int index]
        {
            get
            {
                return Parts[index];
            }
        }

        public IReadOnlyList<T> GetStructuresOfType<T>() where T : IStructureParser
        {
            List<T> result = new List<T>();

            foreach (var part in Parts)
            {
                var instance = Engine.TryGetStructure<T>(part);
                if (instance != null)
                {
                    result.Add(instance);
                }
            }

            return result;
        }

        public T TryGetStructure<T>(int index)
        {
            if (index < 0 || index >= Count)
            {
                return default(T);
            }

            return Engine.TryGetStructure<T>(Parts[index]);
        }

        public int Count
        {
            get
            {
                return Parts.Count;
            }
        }

        public object TryParse(string query)
        {
            if (query.IndexOf(separatorCharArray[0]) != -1)
            {
                var parts = query.Split(separatorCharArray, StringSplitOptions.RemoveEmptyEntries);
                var parsed = parts.Select(p => Engine.Parse(p)).ToArray();
                if (parsed.Any(p => p == null))
                {
                    return null;
                }

                return new SeparatedList(parsed, SeparatorChar);
            }

            return null;
        }

        public override string ToString()
        {
            return string.Join(SeparatorChar.ToString(), Parts.Select(p => p.ToString()));
        }
    }
}
