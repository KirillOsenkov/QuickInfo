using System;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace QuickInfo
{
    public class Query
    {
        public string OriginalInput { get; }
        public bool IsHelp { get; }
        public object Structure { get; }
        public HttpRequest Request { get; set; }

        public Query(string input)
        {
            OriginalInput = input;
            IsHelp = input == "?" || string.Equals(input, "help", StringComparison.OrdinalIgnoreCase);
            Structure = Engine.Parse(input);
        }

        public string IpAddress
        {
            get
            {
                return Request?.HttpContext.Connection.RemoteIpAddress?.ToString() ?? GetIPAddressTheUglyWay(Request?.HttpContext);
            }
        }

        // https://github.com/aspnet/IISIntegration/issues/17
        private string GetIPAddressTheUglyWay(HttpContext httpContext)
        {
            var xForwardedForHeaderValue = httpContext.Request.Headers.GetCommaSeparatedValues("X-FORWARDED-FOR");
            if (xForwardedForHeaderValue != null && xForwardedForHeaderValue.Length > 0)
            {
                IPAddress ipFromHeader;
                var portSeparateLength = xForwardedForHeaderValue[0].LastIndexOf(':');
                var ipAddr = xForwardedForHeaderValue[0].Substring(0, portSeparateLength);
                if (IPAddress.TryParse(ipAddr, out ipFromHeader))
                {
                    return ipFromHeader.ToString();
                }
            }

            return "Cannot determine IP address";
        }

        public T TryGetStructure<T>()
        {
            return Engine.TryGetStructure<T>(Structure);
        }
    }
}
