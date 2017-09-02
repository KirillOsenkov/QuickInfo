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

        public string GetResult(Query query)
        {
            if (triggers.Contains(query.OriginalInput.Trim()))
            {
                return Div("Your IP address: " + query.IpAddress);
            }

            return null;
        }
    }
}
