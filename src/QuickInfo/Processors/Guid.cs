using System;
using System.Collections.Generic;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class RandomGuid : IProcessor
    {
        private HashSet<string> triggerStrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "guid",
            "new guid",
            "guid()",
            "new guid()"
        };

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("guid", "Random Guid"));
            }

            if (triggerStrings.Contains(query.OriginalInput))
            {
                var guid = Guid.NewGuid();
                return new[]
                {
                    Fixed(guid.ToString("D")),
                    Fixed(guid.ToString("D").ToUpperInvariant())
                };
            }

            return null;
        }
    }
}
