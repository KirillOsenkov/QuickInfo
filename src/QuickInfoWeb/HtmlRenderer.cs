using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public class HtmlRenderer
    {
        private static ITextWriter writer;

        private static HtmlRenderer Instance { get; } = new HtmlRenderer();

        public static string RenderObject(object result)
        {
            writer = new StringBuilderTextWriter();
            Instance.Render(result);
            return writer.ToString();
        }

        private void Render(object result)
        {
            switch (result)
            {
                case Node node:
                    RenderNode(node);
                    break;
                case string s:
                    Write(s);
                    break;
                default:
                    throw new NotImplementedException("Can't render " + result);
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
            else if (node.Style == "ColorSwatchSmall")
            {
                return $"background:{node.Text};max-width:60px;height:16px";
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

            return null;
        }

        public string GetText(Node node)
        {
            return null;
        }

        public void RenderText(Node node)
        {
            var text = node.Text;

            if (node.Kind == "Cell" && node.Style == "Color")
            {
                using (SearchLink(text))
                {
                    using (Tag("div", tagClass: "swatch", tagStyle: "background:" + text)) { }
                }

                using (DivClass("swatchName"))
                {
                    Write(text);
                }

                return;
            }

            Write(text);
        }

        public void Write(string text)
        {
            writer.Write(text);
        }

        public void WriteLine(string text)
        {
            writer.WriteLine(text);
        }

        public IDisposable SearchLink(string hyperlink)
        {
            if (hyperlink == null)
            {
                return null;
            }

            var href = HtmlFactory.Attribute("href", "?" + HtmlFactory.UrlEncode(hyperlink));
            var onclick = HtmlFactory.Attribute("onclick", "searchFor(\"" + HtmlFactory.JsEscape(hyperlink) + "\");return false;");
            return Tag("a", tagClass: null, tagStyle: null, (href, onclick));
        }

        public IDisposable DivClass(string tagClass)
        {
            return Tag("div", tagClass, tagStyle: null);
        }

        public void RenderSearchLink(string content, string hyperlink)
        {
            writer.Write(HtmlFactory.SearchLink(content, hyperlink));
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

            using (Tag(tag, nodeClass, nodeStyle))
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
                    using (SearchLink(node.SearchLink))
                    {
                        RenderText(node);
                    }
                }
            }
        }

        private IDisposable Tag(string tag, Node node)
        {
            if (tag == null)
            {
                return null;
            }

            return Tag(tag, GetClass(node), GetStyle(node));
        }

        private IDisposable Tag(string tag, string tagClass, string tagStyle, params (string, string)[] attributes)
        {
            writer.WriteLine(HtmlFactory.TagStart(tag, tagClass, tagStyle, attributes));
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
                writer.WriteLine(HtmlFactory.TagEnd(tag));
            }
        }
    }
}
