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

            if (input.Length == 1)
            {
                return GetResult(input[0]);
            }

            if (input.Length == 2 && char.IsHighSurrogate(input[0]) && char.IsLowSurrogate(input[1]))
            {
                return GetResult(input);
            }

            var bytes = query.TryGetStructure<byte[]>();
            if (bytes != null)
            {
                return GetResult(bytes);
            }

            var utfPrefix = query.TryGetStructure<Prefix>();
            if (utfPrefix != null && utfPrefix.PrefixString.StartsWith("utf"))
            {
                return GetResult(utfPrefix.RemainderString);
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null)
            {
                var sb = new StringBuilder();

                var unicodes = list.GetStructuresOfType<Prefix>();
                foreach (var prefix in unicodes)
                {
                    if (prefix.Remainder is Integer i)
                    {
                        int value = (int)i.Value;
                        char ch = (char)value;
                        sb.Append(ch);
                    }
                }

                if (sb.Length > 0)
                {
                    return GetResult(sb.ToString());
                }
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

            return GetResult(text);
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
            sb.AppendLine(DivClass(Escape(text), "charSample"));
            sb.AppendLine(Div("Unicode code point: " + value));
            sb.AppendLine(DivClass("\\u" + value.ToHex(), "fixed"));
            sb.AppendLine(Div("Category: " + CharUnicodeInfo.GetUnicodeCategory(text[0])));
            sb.AppendLine(GetUtf8(text));
            return sb.ToString();
        }

        private static string GetUtf8(string text)
        {
            return Div("UTF-8: " + string.Join(" ", Encoding.UTF8.GetBytes(text).Select(b => b.ToString("X"))));
        }

        private string GetResult(string text)
        {
            var sb = new StringBuilder();
            sb.AppendLine(DivClass(Escape(text), "charSample"));
            sb.AppendLine(DivClass(GetUtf8(text), "fixed"));
            sb.AppendLine(DivClass(string.Join(" ", text.Select(c => "\\u" + ((int)c).ToHex())), "fixed"));
            return sb.ToString();
        }
    }
}
