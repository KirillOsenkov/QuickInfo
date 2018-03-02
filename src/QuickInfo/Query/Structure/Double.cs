namespace QuickInfo
{
    public class Double : IStructureParser
    {
        public double Value { get; }

        public Double()
        {
        }

        public Double(double i)
        {
            Value = i;
        }

        public object TryParse(string query)
        {
            var trimmed = query.Trim();

            double result = 0;
            if (double.TryParse(trimmed, out result))
            {
                return new Double(result);
            }

            return null;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
