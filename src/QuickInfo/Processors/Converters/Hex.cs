using System.Collections.Generic;
using System.Numerics;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Hex : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable
                (
                    ("0x42a", ""),
                    ("79", "Convert decimal to hex")
                );
            }

            var integer = query.TryGetStructure<Integer>();
            if (integer != null)
            {
                return GetResult(integer);
            }

            var separatedList = query.TryGetStructure<SeparatedList>();
            if (separatedList != null && separatedList.Count >= 2)
            {
                var number = separatedList.TryGetStructure<Integer>(0);
                if (number != null)
                {
                    var keyword1 = separatedList.TryGetStructure<Keyword>(1);
                    if (keyword1 != null)
                    {
                        if (keyword1 == "hex" && separatedList.Count == 2)
                        {
                            return GetResult(number);
                        }

                        if (separatedList.Count == 3 &&
                            (keyword1 == "in" || keyword1 == "to") &&
                            separatedList.TryGetStructure<Keyword>(2) == "hex")
                        {
                            return GetResult(number);
                        }
                    }
                }
                else
                {
                    number = separatedList.TryGetStructure<Integer>(1);
                    var keyword1 = separatedList.TryGetStructure<Keyword>(0);
                    if (number != null && keyword1 != null && keyword1 == "hex" && separatedList.Count == 2)
                    {
                        return GetResult(number);
                    }
                }
            }

            return null;
        }

        private IEnumerable<object> GetResult(Integer value)
        {
            BigInteger bigInteger = value.Value;

            if (value.Kind == IntegerKind.Hexadecimal)
            {
                string hex = bigInteger.ToString("X");
                if (value.OriginalText != null && hex.Length == value.OriginalText.Length)
                {
                    yield return FixedParagraph($"0x{hex} = {bigInteger}");
                }

                if (bigInteger < 0 && value.OriginalText != null)
                {
                    string positiveHex = "0" + value.OriginalText.ToUpperInvariant();
                    if (StringUtilities.TryParseHex(positiveHex, out BigInteger positive))
                    {
                        yield return FixedParagraph($"0x{positiveHex} = {positive}");
                    }
                }
            }
            else
            {
                yield return FixedParagraph($"{bigInteger} = 0x{bigInteger.ToString("X")}");
            }
        }
    }
}
