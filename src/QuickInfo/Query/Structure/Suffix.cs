namespace QuickInfo
{
    public class Suffix : IStructureParser
    {
        public string SuffixString { get; }
        public string PrecedingString { get; }
        public object Remainder { get; }

        public Suffix(string prefix)
        {
            SuffixString = prefix;
        }

        public Suffix(string prefix, string precedingString, object remainder)
        {
            SuffixString = prefix;
            PrecedingString = precedingString;
            Remainder = remainder;
        }

        public object TryParse(string query)
        {
            if (query.Length > SuffixString.Length && query.EndsWith(SuffixString))
            {
                var remainderString = query.Substring(0, query.Length - SuffixString.Length);
                var parsedRemainder = Engine.Parse(remainderString);
                return new Suffix(SuffixString, remainderString, parsedRemainder);
            }

            return null;
        }

        public override string ToString()
        {
            return Remainder.ToString() + SuffixString;
        }
    }
}
