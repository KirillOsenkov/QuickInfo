using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Number = System.Int32;

namespace QuickInfo
{
    public class Factor : IProcessor
    {
        public string GetResult(Query query)
        {
            var integer = query.TryGetStructure<Integer>();
            if (integer != null && integer.Value > -10000000 && integer.Value < 10000000)
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

            if (number < 4 || number > 10000000)
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
