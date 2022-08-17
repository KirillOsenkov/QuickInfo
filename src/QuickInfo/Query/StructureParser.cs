using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickInfo
{
    public class StructureParser
    {
        private List<IStructureParser> structureParsers = new List<IStructureParser>();

        public static StructureParser Instance { get; } = new StructureParser();

        public StructureParser()
        {
            Add(new UnitParser());
            Add(new Keyword("rgb"));
            Add(new Keyword("in"));
            Add(new Keyword("to"));
            Add(new Keyword("hex"));
            Add(new Invocation());
            Add(new Prefix("#"));
            Add(new Prefix("0x"));
            Add(new Prefix("U+", "u+", "\\U", "\\u"));
            Add(new Prefix("utf8 ", "utf-8 ", "utf "));
            Add(new Prefix("unicode ", "char ", "emoji "));
            Add(new Integer());
            Add(new Double());
            Add(new InfixOperator("+"));
            Add(new SeparatedList(','));
            Add(new SeparatedList(' '));
            Add(new RandomInteger());
        }

        public void Add(IStructureParser structureParser)
        {
            structureParsers.Add(structureParser);
        }

        public static object Parse(string input)
        {
            return Instance.ParseWorker(input);
        }

        public static T TryGetStructure<T>(object instance)
        {
            if (instance == null)
            {
                return default(T);
            }

            if (instance is T)
            {
                return (T)instance;
            }

            if (typeof(T) == typeof(Tuple<Double, Unit>) && instance is Tuple<Integer, Unit> intUnit)
            {
                return (T)(object)Tuple.Create((double)intUnit.Item1.Value, intUnit.Item2);
            }

            if (typeof(T) == typeof(byte[]))
            {
                var separatedList = TryGetStructure<SeparatedList>(instance);
                if (separatedList != null)
                {
                    var byteList = separatedList.GetStructuresOfType<Integer>();
                    if (byteList.Count == separatedList.Count && byteList.All(b => b.Value >= -128 && b.Value <= 127))
                    {
                        List<byte> result = new List<byte>();
                        foreach (var b in byteList)
                        {
                            result.Add((byte)b.ForceHexadecimalValue());
                        }

                        return (T)(object)result.ToArray();
                    }
                }
            }

            IEnumerable<object> list = instance as IEnumerable<object>;
            if (list != null)
            {
                foreach (var item in list)
                {
                    var structure = TryGetStructure<T>(item);
                    if (structure != null)
                    {
                        return structure;
                    }
                }
            }

            var integer = instance as Integer;
            if (integer != null && typeof(T) == typeof(Double))
            {
                return (T)(object)new Double(integer.Int32);
            }

            return default(T);
        }

        private object ParseWorker(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            input = input.Trim();

            var list = new List<object>();

            foreach (var parser in structureParsers)
            {
                var result = parser.TryParse(input);
                if (result != null)
                {
                    if (result is Double d && list.Count == 1 && list[0] is Integer integer && ((double)integer.Value) == d.Value)
                    {
                        // skip the double if the int is the same
                    }
                    else
                    {
                        list.Add(result);
                    }
                }
            }

            if (list.Count == 0)
            {
                return null;
            }
            else if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return list;
            }
        }
    }
}
