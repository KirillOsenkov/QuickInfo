using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickInfo
{
    public class NodeFactory
    {
        public static object Text(string text)
        {
            return new Node
            {
                Text = text
            };
        }

        public static Node Paragraph(string text)
        {
            return new Node
            {
                Text = text,
                Kind = "Paragraph"
            };
        }

        public static object HelpTable(params (string, string)[] entries)
        {
            var table = NameValueTable(left => left.SearchLink = left.Text, entries: entries);
            table.Style = "Help";
            return table;
        }

        public static Node NameValueTable(
            Action<Node> leftCellInitializer = null,
            Action<Node> rightCellInitializer = null,
            params (string, string)[] entries)
        {
            var result = new Node
            {
                Kind = "Table",
                List = entries.Select(nameValue =>
                {
                    var leftNode = new Node
                    {
                        Kind = "Cell",
                        Style = "Label",
                        Text = nameValue.Item1
                    };
                    var rightNode = new Node
                    {
                        Kind = "Cell",
                        Text = nameValue.Item2
                    };
                    leftCellInitializer?.Invoke(leftNode);
                    rightCellInitializer?.Invoke(rightNode);
                    return new Node
                    {
                        Kind = "Row",
                        List = new Node[] {leftNode, rightNode}
                    };
                })
            };
            return result;
        }

        public static object SectionHeader(string text)
        {
            return new Node
            {
                Style = "SectionHeader",
                Text = text,
                Kind = "Paragraph"
            };
        }

        public static Node Answer(string text)
        {
            return new Node
            {
                Text = text,
                Kind = "Paragraph",
                Style = "MainAnswer"
            };
        }

        public static object FixedParagraph(string text)
        {
            var node = Fixed(text);
            node.Kind = "Paragraph";
            return node;
        }

        public static Node Fixed(string text)
        {
            return new Node
            {
                Style = "Fixed",
                Text = text
            };
        }

        public static Node Hyperlink(string link, string text = null)
        {
            text = text ?? link;

            return new Node
            {
                Text = text,
                Kind = "Hyperlink",
                Link = link
            };
        }
    }

    public class Node
    {
        private Dictionary<string, object> annotations;
        private Dictionary<string, object> Annotations
        {
            get
            {
                if (annotations == null)
                {
                    annotations = new Dictionary<string, object>();
                }

                return annotations;
            }
        }

        public object this[string index]
        {
            get
            {
                Annotations.TryGetValue(index, out var result);
                return result;
            }

            set
            {
                Annotations[index] = value;
            }
        }

        public string Kind
        {
            get => this[nameof(Kind)] as string;
            set => this[nameof(Kind)] = value;
        }

        public string Style
        {
            get => this[nameof(Style)] as string;
            set => this[nameof(Style)] = value;
        }

        public string Text
        {
            get => this[nameof(Text)] as string;
            set => this[nameof(Text)] = value;
        }

        public string Link
        {
            get => this[nameof(Link)] as string;
            set => this[nameof(Link)] = value;
        }

        public string SearchLink
        {
            get => this[nameof(SearchLink)] as string;
            set => this[nameof(SearchLink)] = value;
        }

        public IEnumerable<object> List
        {
            get => this[nameof(List)] as IEnumerable<object>;
            set => this[nameof(List)] = value;
        }
    }
}
