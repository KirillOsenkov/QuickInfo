using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace QuickInfo
{
    public static class StringUtilities
    {
        public static int[] EnumerateCodePoints(this string text)
        {
            var codePoints = new List<int>(text.Length);

            for (int i = 0; i < text.Length; i++)
            {
                var codePoint = char.ConvertToUtf32(text, i);
                codePoints.Add(codePoint);
                if (char.IsHighSurrogate(text[i]))
                {
                    i += 1;
                }
            }

            return codePoints.ToArray();
        }

        public static bool IsHexOrDecimalChar(this char c)
        {
            return
                (c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        public static bool IsHexChar(this char c)
        {
            return
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        public static bool ContainsHexChars(this string s)
        {
            if (s == null)
            {
                return false;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (IsHexChar(s[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static string ToHex(this char c)
        {
            return ToHex((int)c);
        }

        public static string ToHexByte(this int c)
        {
            return string.Format("{0:X2}", c);
        }

        public static string ToHex(this int i)
        {
            return i.ToString("X");
        }

        public static string ToHex(this BigInteger i)
        {
            return i.ToString("X");
        }

        public static bool TryParseHex(this string s, out int result)
        {
            bool success = int.TryParse("0" + s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
            return success;
        }

        public static bool TryParseHex(this string s, out BigInteger result)
        {
            bool success = BigInteger.TryParse("0" + s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
            return success;
        }

        public static bool IsPrintable(this char c)
        {
            if (char.IsControl(c))
            {
                return false;
            }

            return true;
        }
    }
}
