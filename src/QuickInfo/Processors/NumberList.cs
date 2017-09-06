using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class NumberList : IProcessor
    {
        public string GetResult(Query query)
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

        private string GetResult(SeparatedList originalList, IReadOnlyList<Double> numbersList)
        {
            var sb = new StringBuilder();

            var list = numbersList.Select(l => l.Value).ToList();

            sb.AppendLine(TableStart());
            sb.AppendLine(Row("Sum:", Fixed(list.Sum().ToString())));
            sb.AppendLine(Row("Product:", Fixed(list.Aggregate(1.0, (n, m) => n * m).ToString())));
            sb.AppendLine(Row("Average:", Fixed(list.Average().ToString())));
            sb.AppendLine(Row("Min:", Fixed(list.Min().ToString())));
            sb.AppendLine(Row("Max:", Fixed(list.Max().ToString())));
            sb.AppendLine(Row("Count:", Fixed(list.Count().ToString())));

            list.Sort();
            sb.AppendLine(Row("Sorted:", Fixed(string.Join(", ", list))));

            var integerList = originalList.GetStructuresOfType<Integer>();
            if (integerList != null && integerList.Count == originalList.Count)
            {
                sb.AppendLine(Row("Hex to decimal:", Fixed(string.Join(", ", integerList.Select(l => l.ForceHexadecimalValue().ToString())))));
                if (integerList.All(i => i.Kind == IntegerKind.Decimal))
                {
                    sb.AppendLine(Row("Decimal to hex:", Fixed(string.Join(", ", integerList.Select(l => l.Int32.ToHex())))));
                }
            }

            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}
