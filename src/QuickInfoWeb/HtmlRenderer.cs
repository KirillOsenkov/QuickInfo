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

            return null;
        }

        public string GetClass(Node node)
        {
            return null;
        }

        public string GetStyle(Node node)
        {
            if (node.Style == "Color")
            {
                return "border-spacing: 10px";
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
            var tag = GetTag(node);
            using (Tag(tag, node))
            {
                var list = node.GetList<IEnumerable<object>>();
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        Render(item);
                    }
                }
                else
                {
                    var searchLink = node["SearchLink"] as string;
                    using (SearchLink(searchLink))
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
