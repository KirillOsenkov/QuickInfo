using System.Text;

namespace QuickInfo
{
    public interface ITextWriter
    {
        void Write(string text);
        void WriteLine(string text);
    }

    public class StringBuilderTextWriter : ITextWriter
    {
        private readonly StringBuilder sb;

        public StringBuilderTextWriter()
        {
            sb = new StringBuilder();
        }

        public void Write(string text)
        {
            sb.Append(text);
        }

        public void WriteLine(string text)
        {
            sb.AppendLine(text);
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}
