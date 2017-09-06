using System;
using System.Collections.Generic;
using System.Text;
using static QuickInfo.HtmlFactory;

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

        public string GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("guid", "Random Guid"));
            }

            if (triggerStrings.Contains(query.OriginalInput))
            {
                var guid = Guid.NewGuid();
                return DivClass(guid.ToString("D"), "fixed") +
                       DivClass(guid.ToString("D").ToUpperInvariant(), "fixed");
            }

            return null;
        }
    }
}
