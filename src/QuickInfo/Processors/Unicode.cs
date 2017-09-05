using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Unicode;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Unicode : IProcessor
    {
        public Unicode()
        {
            BuildUnicodeList();
        }

        public string GetResult(Query query)
        {
            var input = query.OriginalInput;

            if (input.Length == 1)
            {
                return GetResult(input[0]);
            }

            if (input.Length == 2 && char.IsHighSurrogate(input[0]) && char.IsLowSurrogate(input[1]))
            {
                return GetResult(char.ConvertToUtf32(input[0], input[1]));
            }

            // naive linear lookup is about 70-80 ms
            // TODO: optimize this?
            var sb = new StringBuilder();
            int hitcount = 0;
            foreach (var d in descriptions)
            {
                if (hitcount > 20)
                {
                    return sb.ToString();
                }

                if (d.Value.IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    sb.AppendLine(DivClass(GetResult(d.Key), "answerSection"));
                    hitcount++;
                }
            }

            if (sb.Length > 0)
            {
                return sb.ToString();
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
                sb = new StringBuilder();

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

        private Dictionary<int, string> descriptions = new Dictionary<int, string>();

        private void BuildUnicodeList()
        {
            var blocks = UnicodeInfo.GetBlocks();

            foreach (var block in blocks)
            {
                foreach (var codepoint in block.CodePointRange)
                {
                    if (char.IsSurrogate((char)codepoint))
                    {
                        continue;
                    }

                    var charInfo = UnicodeInfo.GetCharInfo(codepoint);
                    var displayText = charInfo.Name;
                    if (displayText != null)
                    {
                        descriptions[codepoint] = displayText;
                    }
                }
            }
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
            char ch = (char)value;

            bool isSurrogate = char.IsSurrogate(ch);
            string text = null;
            if (!isSurrogate)
            {
                text = char.ConvertFromUtf32(value);
                sb.AppendLine(DivClass(Escape(text), "charSample"));
            }

            if (descriptions.TryGetValue(value, out string description))
            {
                sb.AppendLine(Div(description));
            }

            var info = UnicodeInfo.GetCharInfo(value);

            sb.AppendLine(Div("Unicode code point: " + value));
            sb.AppendLine(DivClass("\\u" + value.ToHex(), "fixed"));
            sb.AppendLine(Div("Category: " + CharUnicodeInfo.GetUnicodeCategory(ch)));
            sb.AppendLine(Div("Block: " + info.Block));
            if (text != null)
            {
                sb.AppendLine(GetUtf8(text));
            }

            return sb.ToString();
        }

        private static string GetUtf8(string text)
        {
            return Div("UTF-8: " + string.Join(" ", Encoding.UTF8.GetBytes(text).Select(b => b.ToString("X"))));
        }

        private string GetResult(string text)
        {
            if (text.Length == 2 && char.IsHighSurrogate(text[0]) && char.IsLowSurrogate(text[1]))
            {
                return GetResult(char.ConvertToUtf32(text[0], text[1]));
            }

            var sb = new StringBuilder();
            sb.AppendLine(DivClass(Escape(text), "charSample"));
            sb.AppendLine(DivClass(GetUtf8(text), "fixed"));
            sb.AppendLine(DivClass(string.Join(" ", text.Select(c => "\\u" + ((int)c).ToHex())), "fixed"));
            return sb.ToString();
        }
    }
}
