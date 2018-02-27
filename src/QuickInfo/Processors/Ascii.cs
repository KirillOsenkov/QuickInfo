using System;
using System.Collections.Generic;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Ascii : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("ascii", "ASCII table"));
            }

            if (query.OriginalInput.Equals("ascii", StringComparison.OrdinalIgnoreCase))
            {
                return AsciiTable();
            }

            return null;
        }

        private object AsciiTable()
        {
            int columns = 8;
            int columnLength = 256 / columns;

            var headerRow = new List<object>();

            for (int i = 0; i < columns; i++)
            {
                headerRow.Add(new Node
                {
                    Kind = "ColumnHeader",
                    Text = "code",
                    Style = "AsciiColumnHeaderCode"
                });
                headerRow.Add(new Node
                {
                    Kind = "ColumnHeader",
                    Text = "hex",
                    Style = "AsciiColumnHeaderHex"
                });
                headerRow.Add(new Node
                {
                    Kind = "ColumnHeader",
                    Text = "char"
                });
            }

            var rows = new List<object>();
            rows.Add(new Node
            {
                Kind = "Row",
                List = headerRow
            });

            var encoding = Encoding.GetEncoding("latin1");

            for (int i = 0; i < columnLength; i++)
            {
                var row = new List<object>();

                for (int column = 0; column < columns; column++)
                {
                    int character = i + column * columnLength;
                    row.Add(new Node
                    {
                        Kind = "Cell",
                        Style = "AsciiColumnCode",
                        Text = character.ToString()
                    });
                    row.Add(new Node
                    {
                        Kind = "Cell",
                        Style = "AsciiColumnHex",
                        Text = character.ToHex()
                    });

                    var text = encoding.GetString(new byte[] { (byte)character });
                    row.Add(new Node
                    {
                        Kind = "Cell",
                        Text = text,
                        Style = "AsciiColumnChar"
                    });
                }

                rows.Add(new Node
                {
                    Kind = "Row",
                    List = row
                });
            }

            return new Node
            {
                Kind = "Table",
                Style = "Ascii",
                List = rows
            };
        }
    }
}
