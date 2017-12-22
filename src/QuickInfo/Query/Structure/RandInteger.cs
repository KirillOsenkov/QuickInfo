using System;
using System.Text.RegularExpressions;

namespace QuickInfo
{
    public class RandInteger : IStructureParser
    {
        private readonly Regex _regex = new Regex(@"^(?:rnd\*(\d+)|(\d+)\*rnd)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private readonly Random _rnd = new Random();
                                             
        public object TryParse(string query)
        {
            var trimmed = query.Trim();

            var match = _regex.Match(trimmed);

            if (match.Success)
            {
                var numberGroup = !string.IsNullOrEmpty(match.Groups[1].Value) ? match.Groups[1].Value : match.Groups[2].Value;
                int number;
                if (!int.TryParse(numberGroup, out number))
                {
                    return null;
                }
                else
                {                    
                    return number <= 255 ? new Integer(_rnd.Next(number + 1)) : null;
                }
            }

            return null;
        }
    }
}
