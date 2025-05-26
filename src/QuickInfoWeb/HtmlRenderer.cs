using System;
using System.Collections.Generic;
using System.Linq;

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

        private void Render(object result, string processorName = null, string suggestedClass = null)
        {
            switch (result)
            {
                case Node node:
                    RenderNode(node, suggestedClass);
                    break;
                case string s:
                    using (Tag("div", tagClass: "mainAnswerText", multilineContent: false))
                    {
                        Write(WrapMainText(s, processorName));
                    }

                    break;
                case IEnumerable<object> list:
                    RenderList(list, suggestedClass);
                    break;
                case IEnumerable<(string processorName, object resultNode)> tupleList:
                    RenderResults(tupleList);
                    break;
                case null:
                    break;
                default:
                    throw new NotImplementedException("Can't render " + result);
            }
        }

        private void RenderResults(IEnumerable<(string processorName, object resultNode)> results)
        {
            if (!results.Any())
            {
                return;
            }

            if (results.Count() == 1)
            {
                var first = results.First();
                Render(first.resultNode, first.processorName);
                return;
            }

            foreach (var result in results)
            {
                using (DivClass("answerBlock"))
                {
                    using (DivClass("answerBlockHeader"))
                    {
                        Write(result.processorName);
                    }

                    using (DivClass("singleAnswerSection"))
                    {
                        Render(result.resultNode, result.processorName);
                    }
                }
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

        private void RenderNode(Node node, string suggestedClass = null)
        {
            string tag = GetTag(node);
            var list = node.List;
            var nodeClass = GetClass(node) ?? suggestedClass;
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

            string paddingTag = null;
            string paddingClass = null;

            if (node.Style == NodeStyles.Card)
            {
                paddingTag = "div";
                paddingClass = "singleAnswerSection";
            }

            (string, string)[] attributes = null;
            if (node.Link != null)
            {
                attributes = new[]
                {
                    ("href", node.Link),
                    ("target", "_blank")
                };
            }

            var hyperlinkAttributes = tag == "a" ? attributes : null;

            using (Tag(tag, nodeClass, nodeStyle, multilineContent, hyperlinkAttributes))
            {
                IDisposable linkTag = null;
                if (node.Link != null && tag != "a")
                {
                    linkTag = Tag("a", attributes: attributes);
                }

                using var _ = linkTag;

                if (node.Style == NodeStyles.Card && node.Text != null)
                {
                    using (Tag("div", "answerBlockHeader"))
                    {
                        using (DivClass(node[NodeStyles.HeaderStyle] as string))
                        {
                            Write(node.Text);
                        }
                    }
                }

                using (Tag(paddingTag, paddingClass))
                {
                    if (list != null)
                    {
                        string suggestedChildClass = null;
                        if (node.Style == NodeStyles.HorizontalList)
                        {
                            suggestedChildClass = "inlineBlock";
                        }

                        foreach (var item in list)
                        {
                            Render(item, suggestedClass: suggestedChildClass);
                        }
                    }
                    else
                    {
                        RenderContent(node);
                    }
                }
            }
        }

        private void RenderContent(Node node)
        {
            using (SearchLink(node.SearchLink, multilineContent: false))
            {
                if (node.Style == NodeStyles.ColorSwatchSmall)
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

            if (node.Kind == NodeKinds.Cell && node.Style == NodeStyles.Color)
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

        private void RenderList(IEnumerable<object> list, string suggestedClass = null)
        {
            using (Tag("div", tagClass: suggestedClass))
            {
                foreach (var item in list)
                {
                    Render(item);
                }
            }
        }

        public string GetTag(Node node)
        {
            if (node.Kind == NodeKinds.Row)
            {
                return "tr";
            }
            else if (node.Kind == NodeKinds.Cell)
            {
                return "td";
            }
            else if (node.Kind == NodeKinds.Table)
            {
                return "table";
            }
            else if (node.Kind == NodeKinds.ColumnHeader)
            {
                return "th";
            }
            else if (node.Style == NodeStyles.ColorSwatchLarge ||
                     node.Style == NodeStyles.ColorSwatchSmall)
            {
                return "div";
            }
            else if (node.Kind == NodeKinds.Paragraph)
            {
                return "div";
            }
            else if (node.Kind == NodeKinds.Hyperlink)
            {
                return "a";
            }

            return null;
        }

        public string GetClass(Node node)
        {
            if (node.Style == NodeStyles.SectionHeader)
            {
                return "sectionHeader";
            }
            else if (node.Style == NodeStyles.Fixed)
            {
                return "fixed";
            }
            else if (node.Style == NodeStyles.ColorSwatchName)
            {
                return "swatchName";
            }
            else if (node.Style == NodeStyles.MainAnswer)
            {
                return "mainAnswerText";
            }
            else if (node.Style == NodeStyles.Label)
            {
                return "gray";
            }
            else if (node.Style == NodeStyles.CharSample)
            {
                return "charSample";
            }
            else if (node.Style == NodeStyles.Card)
            {
                return "answerBlock";
            }

            return null;
        }

        public string GetStyle(Node node)
        {
            if (node.Style == NodeStyles.Color && node.Kind == NodeKinds.Table)
            {
                return "border-spacing: 10px";
            }
            else if (node.Style == NodeStyles.ColorSwatchLarge)
            {
                return $"background:{node.Text};max-width:300px;height:50px";
            }
            else if (node.Style == NodeStyles.Ascii && node.Kind == NodeKinds.Table)
            {
                return "font-size: 12pt";
            }
            else if (node.Style == NodeStyles.AsciiColumnHeaderCode || node.Style == NodeStyles.AsciiColumnCode)
            {
                return "color: lightseagreen";
            }
            else if (node.Style == NodeStyles.Accent)
            {
                return "color: MediumOrchid";
            }
            else if (node.Style == NodeStyles.AsciiColumnHeaderHex || node.Style == NodeStyles.AsciiColumnHex)
            {
                return "color: lightgray";
            }
            else if (node.Style == NodeStyles.AsciiColumnChar)
            {
                return "column-width: 60px";
            }
            else if (node.Style == NodeStyles.Fixed)
            {
                return "font-size:larger";
            }

            return null;
        }

        public string GetText(Node node)
        {
            if (node.Style == NodeStyles.ColorSwatchLarge || node.Style == NodeStyles.ColorSwatchSmall)
            {
                return null;
            }

            return node.Text;
        }

        public void Write(string text)
        {
            writer.Write(text.Replace("\n", "<br/>"));
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
