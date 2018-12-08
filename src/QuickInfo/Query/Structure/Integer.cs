using System;
using System.Numerics;

namespace QuickInfo
{
    public enum IntegerKind
    {
        Decimal,
        Hexadecimal
    }

    public class Integer : IStructureParser
    {
        public BigInteger Value { get; }
        public IntegerKind Kind { get; private set; }
        public int Int32 => (int)Value;
        public string OriginalText { get; set; }

        public Integer()
        {
        }

        public Integer(BigInteger i)
        {
            Value = i;
        }

        public int ForceHexadecimalValue()
        {
            if (Kind == IntegerKind.Hexadecimal)
            {
                // we're already hex
                return Int32;
            }
            else
            {
                // we originally interpreted the value as decimal;
                // now re-interpret as hex and return
                var hexString = Int32.ToString();
                hexString.TryParseHex(out int hexNumber);
                return hexNumber;
            }
        }

        public bool TryGetInt32(out int int32)
        {
            try
            {
                checked
                {
                    int32 = (int)Value;
                    return true;
                }
            }
            catch
            {
                int32 = 0;
                return false;
            }
        }

        public object TryParse(string query)
        {
            return TryParseInteger(query);
        }

        public static Integer TryParseInteger(string query)
        {
            var trimmed = query.Trim();

            BigInteger result = 0;
            if (BigInteger.TryParse(trimmed, out result))
            {
                return new Integer(result);
            }

            if (trimmed.TryParseHex(out result))
            {
                return new Integer(result)
                {
                    Kind = IntegerKind.Hexadecimal,
                    OriginalText = trimmed
                };
            }

            if (trimmed.Length > 2 && trimmed.StartsWith("0x"))
            {
                trimmed = trimmed.Substring(2);
                if (trimmed.TryParseHex(out result))
                {
                    return new Integer(result)
                    {
                        Kind = IntegerKind.Hexadecimal,
                        OriginalText = trimmed
                    };
                }
            }

            return null;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
