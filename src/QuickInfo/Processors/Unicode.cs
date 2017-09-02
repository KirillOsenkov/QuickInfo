using System;
using System.Globalization;
using System.Linq;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Unicode : IProcessor
    {
        public string GetResult(Query query)
        {
            var input = query.OriginalInput;

            var bytes = query.TryGetStructure<byte[]>();
            if (bytes != null)
            {
                return GetResult(bytes);
            }

            if (input.StartsWith("\\u", StringComparison.OrdinalIgnoreCase) || input.StartsWith("U+", StringComparison.OrdinalIgnoreCase))
            {
                int number;
                if (input.Substring(2).TryParseHex(out number) && IsUnicodeCodepoint(number))
                {
                    return GetResult(number);
                }
            }

            return null;
        }

        private string GetResult(byte[] bytes)
        {
            string text;
            try
            {
                text = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }

            if (text.IndexOf('\ufffd') != -1)
            {
                return null;
            }

            if (text.Any(c => !c.IsPrintable()))
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(Div("UTF-8: " + text));
            return sb.ToString();
        }

        private bool IsUnicodeCodepoint(int number)
        {
            return number >= 0 && number <= 0x10ffff &&
                (number < 0xd800 || number > 0xdfff); // surrogate code points
        }

        private string GetResult(int value)
        {
            var sb = new StringBuilder();
            string text = char.ConvertFromUtf32(value);
            sb.AppendLine(Div(Escape(text), "style=\"font-size: 72px\""));
            sb.AppendLine(Div("Unicode code point: " + value));
            sb.AppendLine(Div("Category: " + CharUnicodeInfo.GetUnicodeCategory(text[0])));
            sb.AppendLine(Div("UTF-8: " + string.Join(" ", Encoding.UTF8.GetBytes(text).Select(b => "0x" + b.ToString("X")))));
            return sb.ToString();
        }
    }
}
