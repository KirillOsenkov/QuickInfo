using System.Net;
using Microsoft.AspNetCore.Http;

namespace QuickInfo
{
    public class WebQuery : Query
    {
        public HttpRequest Request { get; set; }

        public WebQuery(string input) : base(input)
        {
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
    }
}
