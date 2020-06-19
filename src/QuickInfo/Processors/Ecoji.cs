using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public sealed class EcojiEncoder : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("ecoji Hello", Encode("Hello")),
                    ("decode 🏯🔩🚗🌷", "Hello"),
                    ("decoji 🏯🔩🚗🌷", "Hello")
                );
            }

            var input = query.OriginalInput.TrimStart();
            if (input.StartsWith("ecoji "))
            {
                var text = input.Substring(6);
                return Encode(text);
            }
            else if (input.StartsWith("decode ") || input.StartsWith("decoji "))
            {
                var text = input.Substring(7);
                return Decode(text);
            }

            return null;
        }

        public string Encode(string text)
        {
            try
            {
                return Ecoji.Encode(text);
            }
            catch
            {
                return null;
            }
        }

        public string Decode(string text)
        {
            try
            {
                return Ecoji.DecodeUtf8(text);
            }
            catch
            {
                return null;
            }
        }
    }
}
