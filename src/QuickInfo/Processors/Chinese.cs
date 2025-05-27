using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CedictParserLib;
using JiebaNet.Segmenter;
using static QuickInfo.NodeFactory;

namespace QuickInfo;

public class Chinese : IProcessor
{
    private JiebaSegmenter segmenter;
    private JiebaSegmenter Segmenter => segmenter ??= new JiebaSegmenter();

    private Dictionary<string, CedictEntry> dictionary;
    private Dictionary<string, CedictEntry> Dictionary
    {
        get
        {
            if (dictionary == null)
            {
                lock (this)
                {
                    if (dictionary != null)
                    {
                        return dictionary;
                    }

                    dictionary = new Dictionary<string, CedictEntry>();

                    using var parser = new CedictParser(
                        Path.Combine(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "Resources",
                            "cedict_ts.u8"));

                    foreach (var entry in parser.ReadToEnd())
                    {
                        if (entry.Simplified != null)
                        {
                            dictionary[entry.Simplified] = entry;
                        }

                        if (entry.Traditional != null && entry.Traditional != entry.Simplified)
                        {
                            dictionary[entry.Traditional] = entry;
                        }
                    }
                }
            }

            return dictionary;
        }
    }

    public object GetResult(Query query)
    {
        if (query.IsHelp)
        {
            return HelpTable(
                ("我的房子很大", "Chinese"));
        }

        string text = query.OriginalInputTrim;

        if (string.IsNullOrWhiteSpace(text) || text.Length > 1024)
        {
            return null;
        }

        // ignore commas
        text = text.Replace("，", "");
        text = text.Replace("。", "");

        for (int i = 0; i < text.Length; i++)
        {
            char ch = text[i];
            if (!IsChinese(ch))
            {
                return null;
            }
        }

        var words = SplitIntoWords(text);

        var list = new List<object>();

        foreach (var word in words)
        {
            var details = new List<object>();
            if (Dictionary.TryGetValue(word, out var entry))
            {
                if (entry.Pinyin != null)
                {
                    details.Add(Accent(entry.Pinyin));
                }

                if (entry.Definitions != null && entry.Definitions.Length > 0)
                {
                    if (entry.Definitions.Length == 1)
                    {
                        details.Add(Answer(entry.Definitions[0]));
                    }
                    else
                    {
                        var bullets = new Node
                        {
                            List = entry.Definitions,
                            Style = NodeStyles.BulletList
                        };
                        details.Add(bullets);
                    }
                }
            }

            var card = new Node
            {
                Text = word,
                Style = NodeStyles.Card
            };
            card[NodeStyles.HeaderStyle] = "charSample";

            if (details.Count > 0)
            {
                card.List = details;
            }

            list.Add(card);
        }

        return list;
    }

    private IEnumerable<string> SplitIntoWords(string text)
    {
        if (Dictionary.ContainsKey(text))
        {
            return [text];
        }

        var words = Segmenter.Cut(text);
        words = SplitFurther(words).ToArray();
        return words;
    }

    private IEnumerable<string> SplitFurther(IEnumerable<string> words)
    {
        foreach (var word in words)
        {
            if (word.Length == 1)
            {
                yield return word;
            }
            else if (!Dictionary.ContainsKey(word) && word.All(ch => Dictionary.ContainsKey(ch.ToString())))
            {
                foreach (var ch in word)
                {
                    yield return ch.ToString();
                }
            }
            else
            {
                yield return word;
            }
        }
    }

    public static bool IsChinese(char c)
    {
        int code = c;

        // CJK Unified Ideographs (Common Chinese characters)
        if (code >= 0x4E00 && code <= 0x9FFF)
        {
            return true;
        }

        // CJK Unified Ideographs Extension A
        if (code >= 0x3400 && code <= 0x4DBF)
        {
            return true;
        }

        // CJK Unified Ideographs Extension B - F (less common, surrogate pairs needed)
        if (code >= 0x20000 && code <= 0x2EBEF)
        {
            return true;
        }

        // CJK Compatibility Ideographs
        if (code >= 0xF900 && code <= 0xFAFF)
        {
            return true;
        }

        // CJK Radicals Supplement
        if (code >= 0x2E80 && code <= 0x2EFF)
        {
            return true;
        }

        // Kangxi Radicals
        if (code >= 0x2F00 && code <= 0x2FDF)
        {
            return true;
        }

        return false;
    }
}