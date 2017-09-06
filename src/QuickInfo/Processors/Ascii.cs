using System;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Ascii : IProcessor
    {
        public string GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return Table(Row(SearchLink("ascii"), "ASCII table"));
            }

            if (query.OriginalInput.Equals("ascii", StringComparison.OrdinalIgnoreCase))
            {
                return AsciiTable();
            }

            return null;
        }

        private string AsciiTable()
        {
            var sb = new StringBuilder();
            sb.Append("<table style=\"font-size: 12pt\">");
            int columns = 8;
            int columnLength = 256 / columns;

            sb.Append("<tr>");
            var headers = Th("code", "style=\"color: lightseagreen\"") + Th("hex", "style=\"color: lightgray\"") + Th("char");
            for (int i = 0; i < columns; i++)
            {
                sb.Append(headers);
            }

            sb.AppendLine("</tr>");

            for (int i = 0; i < columnLength; i++)
            {
                sb.Append("<tr>");

                for (int column = 0; column < columns; column++)
                {
                    int character = i + column * columnLength;
                    var number = Td(character.ToString(), "style=\"color: lightseagreen\"");
                    var hex = Td(character.ToHex(), "style=\"color: lightgray\"");
                    var encoding = Encoding.GetEncoding("latin1");
                    var text = encoding.GetString(new byte[] { (byte)character });
                    var characterText = Td(Escape(text), "style=\"column-width: 60px\"");
                    sb.Append(number + hex + characterText);
                }

                sb.AppendLine("</tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }
    }
}
