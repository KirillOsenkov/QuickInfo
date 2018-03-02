using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class NumberList : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("16000 41500 3000 2500 12000", "Quick sum of several numbers"),
                    ("3 19 4 -2 7 7 1", "Sum, average, product, sort, min, max"),
                    ("9,1,100,42,0,0,19", "Comma or space delimited sequences"));
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null)
            {
                var numbersList = list.GetStructuresOfType<Double>();
                if (numbersList != null && numbersList.Count == list.Count)
                {
                    return GetResult(list, numbersList);
                }
            }

            return null;
        }

        private object GetResult(SeparatedList originalList, IReadOnlyList<Double> numbersList)
        {
            var list = numbersList.Select(l => l.Value).ToList();

            var pairs = new List<(string, string)>
            {
                ("Sum:", list.Sum().ToString()),
                ("Product:", list.Aggregate(1.0, (n, m) => n * m).ToString()),
                ("Average:", list.Average().ToString()),
                ("Min:", list.Min().ToString()),
                ("Max:", list.Max().ToString()),
                ("Count:", list.Count().ToString()),
                ("Sorted:", string.Join(", ", list.OrderBy(s => s)))
            };

            var integerList = originalList.GetStructuresOfType<Integer>();
            if (integerList != null && integerList.Count == originalList.Count)
            {
                pairs.Add(("Hex to decimal:", string.Join(", ", integerList.Select(l => l.ForceHexadecimalValue().ToString()))));

                if (integerList.All(i => i.Kind == IntegerKind.Decimal))
                {
                    IEnumerable<string> hexList = null;
                    string separator = ", ";
                    if (integerList.All(i => i.Int32 >= 0 && i.Int32 < 256))
                    {
                        hexList = integerList.Select(l => l.Int32.ToHex().PadLeft(2, '0'));
                        separator = " ";
                    }
                    else
                    {
                        hexList = integerList.Select(l => l.Int32.ToHex());
                    }

                    pairs.Add(("Decimal to hex:", string.Join(separator, hexList)));
                }
            }

            return NameValueTable(null, right => right.Style = "Fixed", pairs.ToArray());
        }
    }
}
