using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo.Processors
{
    public class HttpStatusCode : IProcessor
    {
        private static readonly Dictionary<int, string> httpStatusCodes = new Dictionary<int, string>
        {
            [100] = "Continue",
            [101] = "Switching Protocols",
            [200] = "OK",
            [201] = "Created",
            [202] = "Accepted",
            [203] = "Non Authoritative Information",
            [204] = "No Content",
            [205] = "Reset Content",
            [206] = "Partial Content",
            [300] = "Multiple Choices",
            [301] = "Moved Permanently",
            [302] = "Redirect",
            [303] = "See Other",
            [304] = "Not Modified",
            [305] = "Use Proxy",
            [306] = "Unused",
            [307] = "Temporary Redirect",
            [400] = "Bad Request",
            [401] = "Unauthorized",
            [402] = "Payment Required",
            [403] = "Forbidden",
            [404] = "Not Found",
            [405] = "Method Not Allowed",
            [406] = "Not Acceptable",
            [407] = "Proxy Authentication Required",
            [408] = "Request Timeout",
            [409] = "Conflict",
            [410] = "Gone",
            [411] = "Length Required",
            [412] = "Precondition Failed",
            [413] = "Request Entity Too Large",
            [414] = "Request Uri Too Long",
            [415] = "Unsupported Media Type",
            [416] = "Requested Range Not Satisfiable",
            [417] = "Expectation Failed",
            [426] = "Upgrade Required",
            [500] = "Internal Server Error",
            [501] = "Not Implemented",
            [502] = "Bad Gateway",
            [503] = "Service Unavailable",
            [504] = "Gateway Timeout",
            [505] = "Http Version Not Supported"
        };

        public string GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("101", httpStatusCodes[101]));
            }

            if (query.OriginalInput.Equals("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return RenderAll();
            }

            var structure = query.TryGetStructure<Integer>();
            if (structure != null && httpStatusCodes.ContainsKey(structure.Int32))
            {
                return HelpTable(($"{structure.Int32}", httpStatusCodes[structure.Int32]));
            }

            return null;
        }

        private static string RenderAll()
        {
            var sb = new StringBuilder();

            sb.AppendLine(SectionHeader("HTTP status codes:"));

            var rows = from kvp in httpStatusCodes
                       let code = kvp.Key.ToString()
                       select (SearchLink(code, code), kvp.Value);

            sb.AppendLine(NameValueTable(rows.ToArray()));

            return sb.ToString();
        }
    }
}