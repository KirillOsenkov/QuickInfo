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
            if (triggerStrings.Contains(query.OriginalInput))
            {
                var guid = Guid.NewGuid();
                var sb = new StringBuilder();
                sb.Append("<div>");
                sb.Append(Div(guid.ToString("D")));
                sb.Append(Div(guid.ToString("D").ToUpperInvariant()));
                sb.Append("</div>");
                return sb.ToString();
            }

            return null;
        }
    }
}
