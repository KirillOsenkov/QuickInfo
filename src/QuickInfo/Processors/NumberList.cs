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
                    return GetResult(numbersList);
                }
            }

            return null;
        }

        private string GetResult(IReadOnlyList<Double> numbersList)
        {
            var sb = new StringBuilder();

            var list = numbersList.Select(l => l.Value).ToList();

            sb.AppendLine(Div("Sum: " + list.Sum()));
            sb.AppendLine(Div("Product: " + list.Aggregate(1.0, (n, m) => n * m)));
            sb.AppendLine(Div("Average: " + list.Average()));
            sb.AppendLine(Div("Min: " + list.Min()));
            sb.AppendLine(Div("Max: " + list.Max()));
            sb.AppendLine(Div("Count: " + list.Count()));

            list.Sort();
            sb.AppendLine(Div("Sorted: " + string.Join(", ", list)));

            return sb.ToString();
        }
    }
}
