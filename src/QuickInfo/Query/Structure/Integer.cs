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
            TryForceHexadecimalValue(out int result);
            return result;
        }

        /// <summary>
        /// Interprets the original digits as an unsigned hexadecimal number.
        /// Hex text with the high bit set (e.g. "A7FB" or "F0") parses as a negative
        /// two's complement <see cref="Value"/>, so we re-parse the digits with a leading "0".
        /// A decimal like "92" is re-interpreted as hex 0x92.
        /// </summary>
        public bool TryForceHexadecimalValue(out int result)
        {
            string digits;
            if (Kind == IntegerKind.Hexadecimal)
            {
                digits = OriginalText ?? Value.ToString("X");
                if (digits.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    digits = digits.Substring(2);
                }
            }
            else
            {
                digits = Value.ToString();
            }

            return ("0" + digits).TryParseHex(out result);
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
                return new Integer(result)
                {
                    OriginalText = trimmed
                };
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
                string no0x = trimmed.Substring(2);
                if (no0x.TryParseHex(out result))
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
