using System;
using System.Collections.Generic;
using static QuickInfo.HtmlFactory;

namespace QuickInfo.Processors
{
    public class DateTime : IProcessor
    {
        private HashSet<string> triggerStrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "utc",
            "time",
            "time in utc"
        };

        public string GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("utc", "Current UTC date"));
            }

            if (triggerStrings.Contains(query.OriginalInput.Trim()))
            {
                return System.DateTime.UtcNow.ToString();
            }

            return null;
        }
    }
}
