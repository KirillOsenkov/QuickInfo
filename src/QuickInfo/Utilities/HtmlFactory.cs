using System;
using System.Net;
using System.Text;

namespace QuickInfo
{
    public class HtmlFactory
    {
        public static string Escape(string text)
        {
            return WebUtility.HtmlEncode(text);
        }

        public static string UrlEncode(string text)
        {
            return WebUtility.UrlEncode(text);
        }

        public static string TableStart(string tableClass = "")
        {
            if (string.IsNullOrEmpty(tableClass))
            {
                return "<table>";
            }
            else
            {
                return $"<table class=\"{tableClass}\">";
            }
        }

        public static string Tr(params string[] td)
        {
            return "<tr>" + string.Concat(td) + "</tr>";
        }

        public static string Td(string s, string attributes = null)
        {
            if (attributes == null)
            {
                return "<td>" + s + "</td>";
            }
            else
            {
                return "<td " + attributes + ">" + s + "</td>";
            }
        }

        public static string Th(string s, string attributes = null)
        {
            if (attributes == null)
            {
                return "<th>" + s + "</th>";
            }
            else
            {
                return "<th " + attributes + ">" + s + "</th>";
            }
        }

        public static string Gray(string src)
        {
            return DivClass(src, "gray");
        }

        public static string Img(string src)
        {
            return Tag(null, "img", Attribute("src", src));
        }

        public static string H1(string content, params string[] attributes)
        {
            return Tag(content, "h1", attributes);
        }

        public static string SearchLink(string content, string hyperlink)
        {
            var href = Attribute("href", "?" + UrlEncode(hyperlink));
            var onclick = Attribute("onclick", "searchFor(\"" + Escape(hyperlink) + "\");return false;");
            return Tag(content, "a", href, onclick);
        }

        public static string Div(string content, params string[] attributes)
        {
            return Tag(content, "div", attributes);
        }

        public static string DivClass(string content, string className)
        {
            return Div(content, "class=\"" + className + "\"");
        }

        public static string Attribute(string name, object value)
        {
            return name + "=\"" + Escape(Convert.ToString(value)) + "\"";
        }

        public static string Tag(string content, string tag, params string[] attributes)
        {
            var sb = new StringBuilder();
            sb.Append("<");
            sb.Append(tag);
            if (attributes != null && attributes.Length > 0)
            {
                foreach (var attribute in attributes)
                {
                    sb.Append(" ");
                    sb.Append(attribute);
                }
            }

            if (content == null)
            {
                sb.Append(" />");
            }
            else
            {
                sb.Append(">");
                sb.Append(content);
                sb.Append("</");
                sb.Append(tag);
                sb.Append(">");
            }

            return sb.ToString();
        }
    }
}
