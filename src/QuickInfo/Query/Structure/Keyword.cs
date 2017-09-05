using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public class Keyword : IStructureParser
    {
        public string Text { get; }
        public HashSet<string> AlternateSpellings { get; set; }

        public Keyword(string keyword)
        {
            Text = keyword;
        }

        public Keyword(string keyword, params string[] alternateSpellings)
        {
            Text = keyword;
            AlternateSpellings = new HashSet<string>(alternateSpellings, StringComparer.OrdinalIgnoreCase);
        }

        public object TryParse(string query)
        {
            if (string.Equals(query, Text, StringComparison.OrdinalIgnoreCase))
            {
                return this;
            }

            if (AlternateSpellings != null && AlternateSpellings.Contains(query))
            {
                return this;
            }

            return null;
        }

        public override string ToString()
        {
            return Text.ToLowerInvariant();
        }

        public static bool operator ==(Keyword keyword, string text)
        {
            return keyword?.Text == text;
        }

        public static bool operator !=(Keyword keyword, string text)
        {
            return !(keyword == text);
        }

        public override bool Equals(object obj)
        {
            Keyword other = obj as Keyword;
            if (other == null)
            {
                return false;
            }

            return this.Text == other.Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
