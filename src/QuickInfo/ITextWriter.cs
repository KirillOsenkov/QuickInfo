using System.Collections.Generic;
using System.Text;

namespace QuickInfo
{
    public interface ITextWriter
    {
        bool WasLineBreakWritten { get; set; }

        void Write(string text);
        void WriteLine(string text = null);
        void Indent();
        void Unindent();
    }

    public class StringBuilderTextWriter : ITextWriter
    {
        private readonly StringBuilder sb;

        public bool WasLineBreakWritten { get; set; }

        public StringBuilderTextWriter()
        {
            sb = new StringBuilder();
        }

        public void Write(string text)
        {
            if (text == null)
            {
                return;
            }

            WriteIndent();
            sb.Append(text);
            if (text.Contains("\n"))
            {
                WasLineBreakWritten = true;
            }
        }

        private List<string> indentCache = new List<string>();
        private const int indentSize = 2;

        private void WriteIndent()
        {
            if (sb.Length > 0 && sb[sb.Length - 1] == '\n')
            {
                sb.Append(GetIndentString(indent * indentSize));
            }
        }

        private string GetIndentString(int indent)
        {
            string result = null;
            if (indentCache.Count <= indent)
            {
                for (int i = indentCache.Count - 1; i < indent; i++)
                {
                    indentCache.Add(null);
                }
            }

            result = indentCache[indent];
            if (result == null)
            {
                result = new string(' ', indent);
                indentCache[indent] = result;
            }

            return result;
        }

        public void WriteLine(string text = null)
        {
            text = text ?? "";
            WriteIndent();
            sb.AppendLine(text);
            WasLineBreakWritten = true;
        }

        private int indent = 0;

        public void Indent()
        {
            indent++;
        }

        public void Unindent()
        {
            indent--;
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
