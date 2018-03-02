using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public class Prefix : IStructureParser
    {
        public string PrefixString { get; }
        public string RemainderString { get; }
        public object Remainder { get; }
        public HashSet<string> AlternateSpellings { get; set; }
        public string PrefixKind { get; set; }

        public Prefix(string prefix)
        {
            PrefixString = prefix;
            PrefixKind = prefix;
        }

        public Prefix(string prefix, params string[] alternateSpellings)
        {
            PrefixString = prefix;
            PrefixKind = prefix;
            AlternateSpellings = new HashSet<string>(alternateSpellings, StringComparer.OrdinalIgnoreCase);
        }

        public Prefix(string prefixKind, string prefix, string remainderString, object remainder)
        {
            PrefixString = prefix;
            RemainderString = remainderString;
            Remainder = remainder;
            PrefixKind = prefixKind;
        }

        public object TryParse(string query)
        {
            var prefix = TryMatch(query, PrefixString);
            if (prefix != null)
            {
                return prefix;
            }

            if (AlternateSpellings != null)
            {
                foreach (var alternate in AlternateSpellings)
                {
                    prefix = TryMatch(query, alternate);
                    if (prefix != null)
                    {
                        return prefix;
                    }
                }
            }

            return null;
        }

        private Prefix TryMatch(string query, string prefix)
        {
            if (query.Length > prefix.Length && query.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                var remainderString = query.Substring(prefix.Length);
                var parsedRemainder = StructureParser.Parse(remainderString);
                return new Prefix(PrefixKind, prefix, remainderString, parsedRemainder);
            }

            return null;
        }

        public override string ToString()
        {
            return PrefixString + Remainder.ToString();
        }
    }
}
