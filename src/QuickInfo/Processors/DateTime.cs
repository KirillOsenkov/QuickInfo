using System;
using System.Collections.Generic;
using static QuickInfo.NodeFactory;

namespace QuickInfo.Processors
{
    public class DateTime : IProcessor
    {
        private readonly HashSet<string> utcTriggerStrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "utc",
            "time",
            "time in utc"
        };

        private readonly HashSet<string> unixTriggerStrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "unix time",
            "unix epoch",
            "posix time",
            "epoch time"
        };

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("utc", "Current UTC date"),
                    ("unix time", "Current UNIX time"));
            }

            if (utcTriggerStrings.Contains(query.OriginalInput.Trim()))
            {
                return System.DateTime.UtcNow.ToString();
            }

            if (unixTriggerStrings.Contains(query.OriginalInput.Trim()))
            {
                return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            }

            return null;
        }
    }
}
