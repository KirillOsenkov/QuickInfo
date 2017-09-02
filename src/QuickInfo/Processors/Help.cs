using System;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Help : IProcessor
    {
        public string GetResult(Query query)
        {
            if (query.OriginalInput == "?" || string.Equals(query.OriginalInput, "help", StringComparison.OrdinalIgnoreCase))
            {
                return GetResult();
            }

            return null;
        }

        private string GetResult()
        {
            var sb = new StringBuilder();
            foreach (var processor in Engine.Instance.Processors)
            {
                sb.AppendLine(Div(processor.GetType().Name));
            }

            return sb.ToString();
        }
    }
}
