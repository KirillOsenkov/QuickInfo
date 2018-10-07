using System.Linq;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Roman : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable
                (
                    ("MCMLXVIII", "Roman numerals")
                );
            }

            var integer = query.TryGetStructure<Integer>();
            if (integer != null && integer.Int32 > 0 && integer.Int32 < 5000)
            {
                return ToRoman(integer.Int32);
            }

            if (query.OriginalInput.Length > 0 && query.OriginalInput.Length < 17 && query.OriginalInput.All(c => digits.Contains(c)))
            {
                return ParseRoman(query.OriginalInput).ToString();
            }

            return null;
        }

        private static (int arabic, string roman)[] romanNumerals = new[]
        {
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I")
        };

        private static char[] digits = new[] { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };

        private string ToRoman(int value)
        {
            var sb = new StringBuilder();

            foreach (var tuple in romanNumerals)
            {
                while (value >= tuple.arabic)
                {
                    sb.Append(tuple.roman);
                    value -= tuple.arabic;
                }
            }

            return sb.ToString();
        }

        private int ParseRoman(string roman)
        {
            if (string.IsNullOrEmpty(roman))
            {
                return 0;
            }

            int value = 0;

            foreach (var tuple in romanNumerals)
            {
                while (roman.StartsWith(tuple.roman))
                {
                    value += tuple.arabic;
                    roman = roman.Substring(tuple.roman.Length);
                }
            }

            return value;
        }
    }
}
