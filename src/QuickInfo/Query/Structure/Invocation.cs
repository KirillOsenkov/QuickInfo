namespace QuickInfo
{
    public class Invocation : IStructureParser
    {
        public string Prefix { get; }
        public string ArgumentListString { get; }
        public object ArgumentListParsed { get; }

        public Invocation()
        {
        }

        public Invocation(string prefix, string argumentListString, object argumentListParsed)
        {
            Prefix = prefix;
            ArgumentListString = argumentListString;
            ArgumentListParsed = argumentListParsed;
        }

        public object TryParse(string query)
        {
            var trimmed = query.Trim();

            int openParen = trimmed.IndexOf('(');
            int closeParen = trimmed.LastIndexOf(')');
            if (openParen > 0 && closeParen == trimmed.Length - 1)
            {
                var argumentListString = trimmed.Substring(openParen + 1, closeParen - openParen - 1);
                var argumentListParsed = StructureParser.Parse(argumentListString);
                var prefix = trimmed.Substring(0, openParen).TrimEnd();
                return new Invocation(prefix, argumentListString, argumentListParsed);
            }

            return null;
        }

        public override string ToString()
        {
            return Prefix + "(" + ArgumentListParsed.ToString() + ")";
        }
    }
}
