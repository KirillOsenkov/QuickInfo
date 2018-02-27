using System;

namespace QuickInfo
{
    public class Query
    {
        public string OriginalInput { get; }
        public bool IsHelp { get; }
        public object Structure { get; }

        public Query(string input)
        {
            OriginalInput = input;
            IsHelp = input == "?" || string.Equals(input, "help", StringComparison.OrdinalIgnoreCase);
            Structure = StructureParser.Parse(input);
        }

        public T TryGetStructure<T>()
        {
            return StructureParser.TryGetStructure<T>(Structure);
        }
    }
}
