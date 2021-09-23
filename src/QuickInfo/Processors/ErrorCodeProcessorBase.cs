using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public abstract class ErrorCodeProcessorBase : IProcessor
    {
        protected abstract Dictionary<int, (string, string)> Lookup { get; }

        protected abstract int HelpExampleLookupCode { get; }

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                (string Name, string Description) = Lookup[this.HelpExampleLookupCode];
                return NodeFactory.HelpTable(($"0x{this.HelpExampleLookupCode.ToString("X")}", $"{Name} - {Description}"));
            }

            (string Name, string Description) resultInternal = this.LookUpInternal(query);

            if (resultInternal.Name is null && resultInternal.Description is null)
            {
                return null;
            }

            return Encode(resultInternal);
        }

        private (string Name, string Description) LookUpInternal(Query query)
        {
            var structure = query.TryGetStructure<Integer>();
            if (structure != null && Lookup.ContainsKey(structure.Int32))
            {
                return Lookup[structure.Int32];
            }

            return (null, null);
        }

        private static object Encode((string Name, string Description) result)
        {
            List<Object> encodedResult = new List<object>(2);
            encodedResult.Add(NodeFactory.Label(result.Name));
            encodedResult.Add(NodeFactory.FixedParagraph(result.Description));
            return encodedResult;
        }
    }
}
