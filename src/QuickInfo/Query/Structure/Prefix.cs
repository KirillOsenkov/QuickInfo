namespace QuickInfo
{
    public class Prefix : IStructureParser
    {
        public string PrefixString { get; }
        public string RemainderString { get; }
        public object Remainder { get; }

        public Prefix(string prefix)
        {
            PrefixString = prefix;
        }

        public Prefix(string prefix, string remainderString, object remainder)
        {
            PrefixString = prefix;
            RemainderString = remainderString;
            Remainder = remainder;
        }

        public object TryParse(string query)
        {
            if (query.Length > PrefixString.Length && query.StartsWith(PrefixString))
            {
                var remainderString = query.Substring(PrefixString.Length);
                var parsedRemainder = Engine.Parse(remainderString);
                return new Prefix(PrefixString, remainderString, parsedRemainder);
            }

            return null;
        }

        public override string ToString()
        {
            return PrefixString + Remainder.ToString();
        }
    }
}
