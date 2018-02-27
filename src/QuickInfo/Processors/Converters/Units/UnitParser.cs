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
                if (query.EndsWith(keyword.Key, StringComparison.OrdinalIgnoreCase))
                {
                    var parsed = StructureParser.Parse(query.Substring(0, query.Length - keyword.Key.Length));
                    var number = StructureParser.TryGetStructure<Double>(parsed);
                    if (number != null)
                    {
                        return Tuple.Create(number, keyword.Value);
                    }
                }
            }

            return null;
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