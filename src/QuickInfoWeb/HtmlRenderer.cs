using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public class HtmlRenderer
    {
        private ITextWriter writer;

        private static HtmlRenderer Instance { get; } = new HtmlRenderer();

        public static string RenderObject(object result, string processorName = null)
        {
            return Instance.RenderInstance(result, processorName);
        }

        private string RenderInstance(object instance, string processorName = null)
        {
            lock (this)
            {
                writer = new StringBuilderTextWriter();
                Render(instance, processorName);
                var result = writer.ToString();
                writer = null;
                return result;
            }
        }

        private void Render(object result, string processorName = null)
        {
            switch (result)
            {
                case Node node:
                    RenderNode(node);
                    break;
                case string s:
                    using (Tag("div", tagClass: "mainAnswerText", multilineContent: false))
                    {
                        Write(WrapMainText(s, processorName));
                    }

                    break;
                case IEnumerable<object> list:
                    RenderList(list);
                    break;
                case null:
                    break;
                default:
                    throw new NotImplementedException("Can't render " + result);
            }
        }

        private string WrapMainText(string text, string processorName)
        {
            if (processorName == "Roman")
            {
                return HtmlFactory.SpanStyle(text, "font-family: Times New Roman,serif");
            }

            return text;
        }

        private void RenderNode(Node node)
        {
            string tag = GetTag(node);
            var list = node.List;
            var nodeClass = GetClass(node);
            var nodeStyle = GetStyle(node);

            if (tag == null)
            {
                if (list != null)
                {
                    tag = "div";
                }
                else if (nodeClass != null || nodeStyle != null)
                {
                    tag = "span";
                }
            }

            bool multilineContent = list != null;
            var attributes = new List<(string, string)>();
            if (node.Link != null)
            {
                attributes.Add(("href", node.Link));
                attributes.Add(("target", "_blank"));
            }

            using (Tag(tag, nodeClass, nodeStyle, multilineContent, attributes.ToArray()))
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        Render(item);
                    }
                }
                else
                {
                    RenderContent(node);
                }
            }
        }

        private void RenderContent(Node node)
        {
            using (SearchLink(node.SearchLink, multilineContent: false))
            {
                if (node.Style == "ColorSwatchSmall")
                {
                    using (Tag("div", tagStyle: $"background:{node.Text};width:60px;height:16px", multilineContent: false))
                    {
                        return;
                    }
                }

                RenderText(node);
            }
        }

        public void RenderText(Node node)
        {
            var text = node.Text;

            if (node.Kind == "Cell" && node.Style == "Color")
            {
                WriteLine();

                using (SearchLink(text))
                {
                    using (Tag("div", tagClass: "swatch", tagStyle: "background:" + text, multilineContent: false)) { }
                }

                using (DivClass("swatchName", multilineContent: false))
                {
                    Write(text);
                }

                return;
            }

            text = GetText(node);

            if (text != null)
            {
                Write(text);
            }
        }

        private void RenderList(IEnumerable<object> list)
        {
            using (Tag("div"))
            {
                foreach (var item in list)
                {
                    Render(item);
                }
            }
        }

        public string GetTag(Node node)
        {
            if (node.Kind == "Row")
            {
                return "tr";
            }
            else if (node.Kind == "Cell")
            {
                return "td";
            }
            else if (node.Kind == "Table")
            {
                return "table";
            }
            else if (node.Kind == "ColumnHeader")
            {
                return "th";
            }
            else if (node.Style == "ColorSwatchLarge" ||
                     node.Style == "ColorSwatchSmall")
            {
                return "div";
            }
            else if (node.Kind == "Paragraph")
            {
                return "div";
            }
            else if (node.Kind == "Hyperlink")
            {
                return "a";
            }

            return null;
        }

        public string GetClass(Node node)
        {
            if (node.Style == "SectionHeader")
            {
                return "sectionHeader";
            }
            else if (node.Style == "Fixed")
            {
                return "fixed";
            }
            else if (node.Style == "ColorSwatchName")
            {
                return "swatchName";
            }
            else if (node.Style == "MainAnswer")
            {
                return "mainAnswerText";
            }
            else if (node.Style == "Label")
            {
                return "gray";
            }
            else if (node.Style == "CharSample")
            {
                return "charSample";
            }

            return null;
        }

        public string GetStyle(Node node)
        {
            if (node.Style == "Color" && node.Kind == "Table")
            {
                return "border-spacing: 10px";
            }
            else if (node.Style == "ColorSwatchLarge")
            {
                return $"background:{node.Text};max-width:300px;height:50px";
            }
            else if (node.Style == "Ascii" && node.Kind == "Table")
            {
                return "font-size: 12pt";
            }
            else if (node.Style == "AsciiColumnHeaderCode" || node.Style == "AsciiColumnCode")
            {
                return "color: lightseagreen";
            }
            else if (node.Style == "AsciiColumnHeaderHex" || node.Style == "AsciiColumnHex")
            {
                return "color: lightgray";
            }
            else if (node.Style == "AsciiColumnChar")
            {
                return "column-width: 60px";
            }
            else if (node.Style == "Fixed")
            {
                return "font-size:larger";
            }

            return null;
        }

        public string GetText(Node node)
        {
            if (node.Style == "ColorSwatchLarge" || node.Style == "ColorSwatchSmall")
            {
                return null;
            }

            return node.Text;
        }

        public void Write(string text)
        {
            writer.Write(text);
        }

        public void WriteLine(string text = null)
        {
            writer.WriteLine(text);
        }

        public IDisposable SearchLink(string hyperlink, bool multilineContent = true)
        {
            if (hyperlink == null)
            {
                return null;
            }

            var href = "?" + HtmlFactory.UrlEncode(hyperlink);
            var onclick = "searchFor(\"" + HtmlFactory.JsEscape(hyperlink) + "\");return false;";
            return Tag("a", tagClass: null, tagStyle: null, multilineContent: multilineContent, ("href", href), ("onclick", onclick));
        }

        public IDisposable DivClass(string tagClass, bool multilineContent = true)
        {
            return Tag("div", tagClass, tagStyle: null, multilineContent: multilineContent);
        }

        public void RenderSearchLink(string content, string hyperlink)
        {
            writer.Write(HtmlFactory.SearchLink(content, hyperlink));
        }

        private IDisposable Tag(string tag, Node node)
        {
            if (tag == null)
            {
                return null;
            }

            return Tag(tag, GetClass(node), GetStyle(node));
        }

        private IDisposable Tag(string tag, string tagClass = null, string tagStyle = null, bool multilineContent = true, params (string, string)[] attributes)
        {
            if (tag != null)
            {
                var tagStart = HtmlFactory.TagStart(tag, tagClass, tagStyle, attributes);
                if (multilineContent)
                {
                    tagStart += Environment.NewLine;
                }

                writer.Write(tagStart);
                writer.Indent();
            }

            return new TagDisposable(tag, writer);
        }

        private class TagDisposable : IDisposable
        {
            private string tag;
            private ITextWriter writer;

            public TagDisposable(string tag, ITextWriter writer)
            {
                this.tag = tag;
                this.writer = writer;
            }

            public void Dispose()
            {
                if (tag != null)
                {
                    writer.Unindent();
                    writer.WriteLine(HtmlFactory.TagEnd(tag));
                }
            }
        }
    }
}
