using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Unicode;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Unicode : IProcessor
    {
        const int MaxSymbolsToReturn = 60;

        private Dictionary<int, string> descriptions = new Dictionary<int, string>();
        private (string, int)[] index;

        public Unicode()
        {
            BuildUnicodeList();
        }

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("cherries", "Lookup a unicode char/emoji"),
                    ("\\U0001F352", "Lookup char"),
                    ("\\U0001F347 \\U0001F352", "Lookup multiple chars"),
                    ("🍒", "Show char info"),
                    ("F0 9F 8D 92 F0 9F 8D 87", "Decode from UTF-8 bytes"),
                    ("utf8 пример", "Encode in UTF-8"));
            }

            var input = query.OriginalInput;

            if (input.Length == 1)
            {
                return GetResult(input[0]);
            }

            if (input.Length == 2 && char.IsHighSurrogate(input[0]) && char.IsLowSurrogate(input[1]))
            {
                return GetResult(char.ConvertToUtf32(input[0], input[1]));
            }

            var prefix = query.TryGetStructure<Prefix>();
            if (prefix != null)
            {
                if (prefix.PrefixKind == "utf8 ")
                {
                    return GetResult(prefix.RemainderString);
                }

                if (prefix.PrefixKind == "unicode ")
                {
                    var lookup = LookupUnicodeCharacter(prefix.RemainderString);
                    if (lookup != null)
                    {
                        return lookup;
                    }
                }

                if (prefix.PrefixKind == "U+")
                {
                    var integer = StructureParser.TryGetStructure<Integer>(prefix.Remainder);
                    if (integer != null && integer.ForceHexadecimalValue() is int hexValue && IsUnicodeCodepoint(hexValue))
                    {
                        return GetResult(hexValue);
                    }
                }
            }

            var bytes = query.TryGetStructure<byte[]>();
            if (bytes != null)
            {
                return GetResult(bytes);
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null)
            {
                var sb = new StringBuilder();

                var codepoints = list.GetStructuresOfType<Prefix>();
                foreach (var uPrefix in codepoints)
                {
                    var integer = StructureParser.TryGetStructure<Integer>(uPrefix.Remainder);
                    if (integer != null && integer.ForceHexadecimalValue() is int hexValue && IsUnicodeCodepoint(hexValue))
                    {
                        sb.Append(char.ConvertFromUtf32(hexValue));
                    }
                }

                if (sb.Length > 0)
                {
                    return GetResult(sb.ToString());
                }
            }

            var positions = SortedSearch.FindItems(index, input.SplitIntoWords(), t => t.Item1, t => t.Item2);
            if (positions.Any())
            {
                var characters = positions
                    .OrderBy(i => i)
                    .Take(MaxSymbolsToReturn)
                    .Select(i => GetResult(i))
                    .ToArray();

                return characters;
            }

            return null;
        }

        private object LookupUnicodeCharacter(string input)
        {
            // naive linear lookup is about 70-80 ms
            // TODO: optimize this?
            List<object> resultCards = new List<object>();
            int hitcount = 0;
            foreach (var d in descriptions)
            {
                if (hitcount >= MaxSymbolsToReturn)
                {
                    return RenderResultCards(resultCards);
                }

                if (d.Value.IndexOf(input, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    resultCards.Add(GetResult(d.Key));
                    hitcount++;
                }
            }

            return RenderResultCards(resultCards);
        }

        private object RenderResultCards(List<object> resultCards)
        {
            if (resultCards.Count == 0)
            {
                return null;
            }

            if (resultCards.Count == 1)
            {
                return resultCards[0];
            }

            return resultCards;
        }

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

            index = SortedSearch.CreateIndex(descriptions.Select(kvp => (kvp.Value, kvp.Key)), s => s.SplitIntoWords());
        }

        private object GetResult(byte[] bytes)
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

        private object GetResult(int value)
        {
            char ch = (char)value;

            var result = new List<object>();

            bool isSurrogate = char.IsSurrogate(ch);
            string text = null;
            if (!isSurrogate)
            {
                text = char.ConvertFromUtf32(value);
                var answer = Answer(text);
                answer.Style = "CharSample";
                result.Add(answer);
            }

            if (descriptions.TryGetValue(value, out string description))
            {
                result.Add(Answer(description));
            }

            var info = UnicodeInfo.GetCharInfo(value);

            var pairs = new List<(string, string)>
            {
                ("Code point:", $"{value} (U+{value.ToHex()})"),
                ("Category:", CharUnicodeInfo.GetUnicodeCategory(ch).ToString()),
                ("Block:", info.Block),
                ("Escape:", GetEscapeString(value))
            };

            if (text != null)
            {
                pairs.Add(("UTF-8:", GetUtf8(text)));
            }

            result.Add(NameValueTable(null, right => right.Style = "Fixed", entries: pairs.ToArray()));

            return result;
        }

        private static string GetEscapeString(int value)
        {
            if (char.ConvertFromUtf32(value).Length == 2)
            {
                return "\\U" + value.ToHex().PadLeft(8, '0');
            }
            else
            {
                return "\\u" + value.ToHex().PadLeft(4, '0');
            }
        }

        private static string GetUtf8(string text)
        {
            return string.Join(" ", Encoding.UTF8.GetBytes(text).Select(b => b.ToString("X")));
        }

        private object GetResult(string text)
        {
            if (text.Length == 2 && char.IsHighSurrogate(text[0]) && char.IsLowSurrogate(text[1]))
            {
                return GetResult(char.ConvertToUtf32(text[0], text[1]));
            }

            var list = new List<object>();
            var answer = Answer(text);
            answer.Style = "CharSample";
            list.Add(answer);
            foreach (var codepoint in text.EnumerateCodePoints().Select(c => GetEscapeString(c)))
            {
                var link = Fixed(codepoint);
                link.SearchLink = codepoint;
                list.Add(link);
            }

            list.Add(FixedParagraph(GetUtf8(text)));

            return list;
        }
    }
}
