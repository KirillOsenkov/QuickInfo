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

        public static object Paragraph(string text)
        {
            return new Node
            {
                Text = text,
                Kind = "Paragraph"
            };
        }

        public static object HelpTable(params (string, string)[] entries)
        {
            var table = NameValueTable(entries);
            table.Style = "Help";
            return table;
        }

        public static Node NameValueTable(params (string, string)[] entries)
        {
            var result = new Node
            {
                Kind = "Table",
                List = entries.Select(nameValue => new Node
                {
                    Kind = "Row",
                    List = new Node[]
                    {
                        new Node
                        {
                            Kind = "Cell",
                            Text = nameValue.Item1,
                            SearchLink = nameValue.Item1
                        },
                        new Node
                        {
                            Kind = "Cell",
                            Text = nameValue.Item2
                        }
                    }
                })
            };
            return result;
        }

        public static object SectionHeader(string text)
        {
            return new Node
            {
                Style = "SectionHeader",
                Text = text
            };
        }

        public static object Fixed(string text)
        {
            return new Node
            {
                Style = "Fixed",
                Text = text
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
