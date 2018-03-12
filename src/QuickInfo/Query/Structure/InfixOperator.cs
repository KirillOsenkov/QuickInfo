namespace QuickInfo
{
    public class InfixOperator : IStructureParser
    {
        public string OperatorText;

        public string LeftString;
        public string RightString;
        public object Left;
        public object Right;

        public InfixOperator(string operatorText)
        {
            this.OperatorText = operatorText;
        }

        public InfixOperator(
            string leftString,
            object left,
            string operatorText,
            string rightString,
            object right) : this(operatorText)
        {
            LeftString = leftString;
            RightString = rightString;
            Left = left;
            Right = right;
        }

        public object TryParse(string query)
        {
            var index = query.IndexOf(OperatorText);
            if (index < 1 || index == query.Length - OperatorText.Length)
            {
                return null;
            }

            LeftString = query.Substring(0, index);
            RightString = query.Substring(index + OperatorText.Length, query.Length - index - OperatorText.Length);
            var left = StructureParser.Parse(LeftString);
            var right = StructureParser.Parse(RightString);

            if (left != null && right != null)
            {
                return new InfixOperator(LeftString, left, OperatorText, RightString, right);
            }

            return null;
        }
    }
}
