using System;
using System.Collections.Generic;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Ip : IProcessor
    {
        private static HashSet<string> triggers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ip",
            "ip address",
            "my ip",
            "what is my ip"
        };

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("ip", "Your IP address"));
            }

            if (query is WebQuery webQuery)
            {
                if (triggers.Contains(query.OriginalInput.Trim()))
                {
                    return Table(Row("Your IP address:", webQuery.IpAddress));
                }
            }

            return null;
        }
    }
}
