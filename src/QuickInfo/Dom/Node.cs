using System.Collections.Generic;
using System.Linq;

namespace QuickInfo
{
    public class NodeFactory
    {
        public static object HelpTable(params (string, string)[] entries)
        {
            var result = new Node
            {
                ["Kind"] = "Table",
                ["Style"] = "Help",
                ["List"] = entries.Select(nameValue => new Node
                {
                    ["Kind"] = "Row",
                    ["List"] = new Node[]
                    {
                        new Node
                        {
                            ["Kind"] = "Cell",
                            ["Text"] = nameValue.Item1,
                            ["SearchLink"] = nameValue.Item1
                        },
                        new Node
                        {
                            ["Kind"] = "Cell",
                            ["Text"] = nameValue.Item2
                        }
                    }
                })
            };
            return result;
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

        public IEnumerable<T> GetList<T>()
        {
            return this["List"] as IEnumerable<T>;
        }
    }
}
