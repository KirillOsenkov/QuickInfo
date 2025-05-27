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
                Kind = NodeKinds.Paragraph
            };
        }

        public static Node HorizontalList(IEnumerable<object> list)
        {
            return new Node
            {
                List = list,
                Style = NodeStyles.HorizontalList
            };
        }

        public static object HelpTable(params (string, string)[] entries)
        {
            var table = NameValueTable(left => left.SearchLink = left.Text, entries: entries);
            table.Style = NodeStyles.Help;
            return table;
        }

        public static Node NameValueTable(
            Action<Node> leftCellInitializer = null,
            Action<Node> rightCellInitializer = null,
            params (string, string)[] entries)
        {
            var result = new Node
            {
                Kind = NodeKinds.Table,
                List = entries.Select(nameValue =>
                {
                    var leftNode = new Node
                    {
                        Kind = NodeKinds.Cell,
                        Style = NodeStyles.Label,
                        Text = nameValue.Item1
                    };
                    var rightNode = new Node
                    {
                        Kind = NodeKinds.Cell,
                        Text = nameValue.Item2
                    };
                    leftCellInitializer?.Invoke(leftNode);
                    rightCellInitializer?.Invoke(rightNode);
                    return new Node
                    {
                        Kind = NodeKinds.Row,
                        List = new Node[] { leftNode, rightNode }
                    };
                })
            };
            return result;
        }

        public static object SectionHeader(string text)
        {
            return new Node
            {
                Style = NodeStyles.SectionHeader,
                Text = text,
                Kind = NodeKinds.Paragraph
            };
        }

        public static Node Answer(string text)
        {
            return new Node
            {
                Text = text,
                Kind = NodeKinds.Paragraph,
                Style = NodeStyles.MainAnswer
            };
        }

        public static Node Accent(string text)
        {
            return new Node
            {
                Text = text,
                Style = NodeStyles.Accent
            };
        }

        public static Node[] Answers(params string[] lines)
        {
            return lines.Select(Answer).ToArray();
        }

        public static object FixedParagraph(string text)
        {
            var node = Fixed(text);
            node.Kind = NodeKinds.Paragraph;
            return node;
        }

        public static Node Fixed(string text)
        {
            return new Node
            {
                Style = NodeStyles.Fixed,
                Text = text
            };
        }

        public static Node Label(string text)
        {
            return new Node
            {
                Style = NodeStyles.Label,
                Text = text
            };
        }

        public static Node Hyperlink(string link, string text = null)
        {
            text = text ?? link;

            return new Node
            {
                Text = text,
                Kind = NodeKinds.Hyperlink,
                Link = link
            };
        }

        public static Node SearchLink(string search)
        {
            return new Node
            {
                Text = search,
                SearchLink = search
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

    public class NodeKinds
    {
        public const string Hyperlink = nameof(Hyperlink);
        public const string Paragraph = nameof(Paragraph);
        public const string Table = nameof(Table);
        public const string Cell = nameof(Cell);
        public const string Row = nameof(Row);
        public const string ColumnHeader = nameof(ColumnHeader);
    }

    public class NodeStyles
    {
        public const string Help = nameof(Help);
        public const string Label = nameof(Label);
        public const string SectionHeader = nameof(SectionHeader);
        public const string HeaderStyle = nameof(HeaderStyle);
        public const string MainAnswer = nameof(MainAnswer);
        public const string Fixed = nameof(Fixed);
        public const string AsciiColumnHeaderCode = nameof(AsciiColumnHeaderCode);
        public const string AsciiColumnHeaderHex = nameof(AsciiColumnHeaderHex);
        public const string AsciiColumnCode = nameof(AsciiColumnCode);
        public const string AsciiColumnHex = nameof(AsciiColumnHex);
        public const string AsciiColumnChar = nameof(AsciiColumnChar);
        public const string Ascii = nameof(Ascii);
        public const string Accent = nameof(Accent);
        public const string Color = nameof(Color);
        public const string ColorSwatchLarge = nameof(ColorSwatchLarge);
        public const string ColorSwatchSmall = nameof(ColorSwatchSmall);
        public const string ColorSwatchName = nameof(ColorSwatchName);
        public const string CharSample = nameof(CharSample);
        public const string HorizontalList = nameof(HorizontalList);
        public const string Card = nameof(Card);
        public const string BulletList = nameof(BulletList);
    }
}
