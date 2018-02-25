using System.Numerics;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Hex : IProcessor
    {
        public string GetResult(Query query)
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
                return GetResult(integer.Value);
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
                            return GetResult(number.Value);
                        }

                        if (separatedList.Count == 3 &&
                            (keyword1 == "in" || keyword1 == "to") &&
                            separatedList.TryGetStructure<Keyword>(2) == "hex")
                        {
                            return GetResult(number.Value);
                        }
                    }
                }
                else
                {
                    number = separatedList.TryGetStructure<Integer>(1);
                    var keyword1 = separatedList.TryGetStructure<Keyword>(0);
                    if (number != null && keyword1 != null && keyword1 == "hex" && separatedList.Count == 2)
                    {
                        return GetResult(number.Value);
                    }
                }
            }

            return null;
        }

        private string GetResult(BigInteger value)
        {
            return Div(Escape($"{value} = 0x{value.ToString("X")}"));
        }
    }
}
