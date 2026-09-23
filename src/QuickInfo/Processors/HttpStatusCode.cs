using System;
using System.Collections.Generic;
using System.Linq;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class HttpStatusCode : IProcessor
    {
        private static readonly Dictionary<int, string> httpStatusCodes = new Dictionary<int, string>
        {
            [100] = "Continue",
            [101] = "Switching Protocols",
            [102] = "Processing",
            [103] = "Early Hints",
            [200] = "OK",
            [201] = "Created",
            [202] = "Accepted",
            [203] = "Non Authoritative Information",
            [204] = "No Content",
            [205] = "Reset Content",
            [206] = "Partial Content",
            [207] = "Multi-Status",
            [208] = "Already Reported",
            [226] = "IM Used",
            [300] = "Multiple Choices",
            [301] = "Moved Permanently",
            [302] = "Redirect",
            [303] = "See Other",
            [304] = "Not Modified",
            [305] = "Use Proxy",
            [306] = "Unused",
            [307] = "Temporary Redirect",
            [308] = "Permanent Redirect",
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
            [418] = "I'm a teapot",
            [421] = "Misdirected Request",
            [422] = "Unprocessable Content",
            [423] = "Locked",
            [424] = "Failed Dependency",
            [425] = "Too Early",
            [426] = "Upgrade Required",
            [428] = "Precondition Required",
            [429] = "Too Many Requests",
            [431] = "Request Header Fields Too Large",
            [451] = "Unavailable For Legal Reasons",
            [500] = "Internal Server Error",
            [501] = "Not Implemented",
            [502] = "Bad Gateway",
            [503] = "Service Unavailable",
            [504] = "Gateway Timeout",
            [505] = "Http Version Not Supported",
            [506] = "Variant Also Negotiates",
            [507] = "Insufficient Storage",
            [508] = "Loop Detected",
            [510] = "Not Extended",
            [511] = "Network Authentication Required"
        };

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("101", httpStatusCodes[101]),
                    ("http", "All http status codes"));
            }

            if (query.OriginalInput.Equals("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return RenderAll();
            }

            var structure = query.TryGetStructure<Integer>();
            if (structure != null &&
                structure.Value >= 100 &&
                structure.Value <= 511 &&
                httpStatusCodes.ContainsKey(structure.Int32))
            {
                return Answer($"http code {structure.Int32}: {httpStatusCodes[structure.Int32]}");
            }

            return null;
        }

        private static object RenderAll()
        {
            var list = new List<object>();

            list.Add(SectionHeader("HTTP status codes:"));

            var rows = from kvp in httpStatusCodes
                       let code = kvp.Key.ToString()
                       select (code, kvp.Value);

            list.Add(NameValueTable(entries: rows.ToArray()));

            return list;
        }
    }
}
