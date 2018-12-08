using System.Collections.Generic;
using System.Text;
using Number = System.Int32;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Factor : IProcessor
    {
        private const int Max = 100_000_000;

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(("2520", "Factor integers (< 10,000,000)"));
            }

            var integer = query.TryGetStructure<Integer>();
            if (integer != null &&
                integer.Kind != IntegerKind.Hexadecimal &&
                integer.Value > 0 &&
                integer.Value < Max)
            {
                return GetResult((int)integer.Value);
            }

            return null;
        }

        private string GetResult(Number number)
        {
            if (number < 0)
            {
                number = -number;
            }

            if (number < 4 || number > Max)
            {
                return null;
            }

            var factors = new List<Number>();
            var bound = number / 2;
            for (Number i = 2; i <= bound; i++)
            {
                if (i > 3)
                {
                    // skip further even divisors
                    i++;
                }

                while (number % i == 0)
                {
                    number /= i;
                    factors.Add(i);
                }

                if (number == 1)
                {
                    break;
                }
            }

            if (number != 1)
            {
                return "Prime number.";
            }

            var sb = new StringBuilder();
            sb.Append("Factors: ");

            sb.Append(string.Join(" ", factors));

            return sb.ToString();
        }
    }
}
