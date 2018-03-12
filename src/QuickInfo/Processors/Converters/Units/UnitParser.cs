using System;
using System.Collections.Generic;

namespace QuickInfo
{
    public class UnitParser : IStructureParser
    {
        public object TryParse(string query)
        {
            var result = Parse(query);
            if (result != null)
            {
                return result;
            }

            foreach (var keyword in keywords)
            {
                Double number = null;
                if (query.EndsWith(keyword.Key, StringComparison.OrdinalIgnoreCase))
                {
                    number = TryParseUnitValue(query, 0, query.Length - keyword.Key.Length);
                }
                else if (IsPrefixUnit(keyword.Key) && query.StartsWith(keyword.Key))
                {
                    number = TryParseUnitValue(query, keyword.Key.Length, query.Length - keyword.Key.Length);
                }

                if (number != null)
                {
                    return Tuple.Create(number, keyword.Value);
                }
            }

            return null;
        }

        public static bool IsPrefixUnit(string unitName) => unitName == "$";

        private static Double TryParseUnitValue(string query, int start, int length)
        {
            var parsed = StructureParser.Parse(query.Substring(start, length));
            var number = StructureParser.TryGetStructure<Double>(parsed);
            if (number != null)
            {
                if (StructureParser.TryGetStructure<Integer>(parsed) is Integer i && i.Kind == IntegerKind.Hexadecimal)
                {
                    // don't allow hex numbers with units
                    return null;
                }
            }

            return number;
        }

        public static object Parse(string unitName)
        {
            Unit unit = null;
            keywords.TryGetValue(unitName, out unit);
            return unit;
        }

        static UnitParser()
        {
            foreach (var unit in Units.AllUnits)
            {
                foreach (var name in unit.Names)
                {
                    if (!keywords.ContainsKey(name))
                    {
                        keywords.Add(name, unit);
                    }
                }
            }
        }

        private static readonly Dictionary<string, Unit> keywords = new Dictionary<string, Unit>(StringComparer.OrdinalIgnoreCase);
    }
}
