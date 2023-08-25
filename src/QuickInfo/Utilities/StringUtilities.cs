using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            bool success = int.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
            return success;
        }

        public static bool TryParseHex(this string s, out BigInteger result)
        {
            // Prepend "0" to ensure that the hex string is parsed as a positive number
            // See https://docs.microsoft.com/en-us/dotnet/api/system.numerics.biginteger#working-with-byte-arrays-and-hexadecimal-strings
            bool success = BigInteger.TryParse(s, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out result);
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

        public static IEnumerable<IEnumerable<T>> AsJagged<T>(this T[,] array)
        {
            for (int i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
            {
                yield return GetRow(array, i);
            }
        }

        public static IEnumerable<T> GetRow<T>(this T[,] array, int row)
        {
            for (int column = array.GetLowerBound(1); column <= array.GetUpperBound(1); column++)
            {
                yield return array[row, column];
            }
        }

        private static readonly char[] space = new[] { ' ' };
        public static IReadOnlyList<string> SplitIntoWords(this string text)
        {
            return text.Split(space, StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool IsSingleWord(this string word)
        {
            return word.All(c => char.IsLetter(c));
        }

        public static string NormalizeLineBreaks(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            return text;
        }

        public static IReadOnlyList<string> GetLines(this string text, bool includeLineBreak = false)
        {
            return GetLineSpans(text, includeLineBreakInSpan: includeLineBreak)
                .Select(span => text.Substring(span.Start, span.Length))
                .ToArray();
        }

        public static IReadOnlyList<Span> GetLineSpans(this string text, bool includeLineBreakInSpan = true)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.Length == 0)
            {
                return Empty;
            }

            var result = new List<Span>();
            text.CollectLineSpans(result, includeLineBreakInSpan);
            return result.ToArray();
        }

        public static void CollectLineSpans(this string text, ICollection<Span> spans, bool includeLineBreakInSpan = true)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (spans == null)
            {
                throw new ArgumentNullException(nameof(spans));
            }

            if (text.Length == 0)
            {
                return;
            }

            int currentPosition = 0;
            int currentLineLength = 0;
            bool previousWasCarriageReturn = false;

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (ch == '\r')
                {
                    if (previousWasCarriageReturn)
                    {
                        int lineLengthIncludingLineBreak = currentLineLength;
                        if (!includeLineBreakInSpan)
                        {
                            currentLineLength--;
                        }

                        spans.Add(new Span(currentPosition, currentLineLength));

                        currentPosition += lineLengthIncludingLineBreak;
                        currentLineLength = 1;
                    }
                    else
                    {
                        currentLineLength++;
                        previousWasCarriageReturn = true;
                    }
                }
                else if (ch == '\n')
                {
                    var lineLength = currentLineLength;
                    if (previousWasCarriageReturn)
                    {
                        lineLength--;
                    }

                    currentLineLength++;
                    previousWasCarriageReturn = false;
                    if (includeLineBreakInSpan)
                    {
                        lineLength = currentLineLength;
                    }

                    spans.Add(new Span(currentPosition, lineLength));
                    currentPosition += currentLineLength;
                    currentLineLength = 0;
                }
                else
                {
                    if (previousWasCarriageReturn)
                    {
                        var lineLength = currentLineLength;
                        if (!includeLineBreakInSpan)
                        {
                            lineLength--;
                        }

                        spans.Add(new Span(currentPosition, lineLength));
                        currentPosition += currentLineLength;
                        currentLineLength = 0;
                        previousWasCarriageReturn = false;
                    }

                    currentLineLength++;
                }
            }

            var finalLength = currentLineLength;
            if (previousWasCarriageReturn && !includeLineBreakInSpan)
            {
                finalLength--;
            }

            spans.Add(new Span(currentPosition, finalLength));

            if (previousWasCarriageReturn)
            {
                spans.Add(new Span(currentPosition, 0));
            }
        }

        private static readonly IReadOnlyList<Span> Empty = new Span[] { Span.Empty };
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string TrimWhitespaceFromEachLine(this string text)
        {
            if (text == null)
            {
                return null;
            }

            var lines = text.GetLines();
            var trimmed = lines.Select(s => s.Trim());
            var joined = string.Join("\n", trimmed);
            return joined;
        }
    }

    public struct Span
    {
        public int Start;
        public int Length;
        public int End => Start + Length;

        public static readonly Span Empty = new Span();

        public Span(int start, int length) : this()
        {
            Start = start;
            Length = length;
        }

        public override string ToString()
        {
            return $"({Start}, {Length})";
        }

        public Span Skip(int length)
        {
            if (length > Length)
            {
                return new Span();
            }

            return new Span(Start + length, Length - length);
        }

        public bool Contains(int position)
        {
            return position >= Start && position <= End;
        }
    }
}
