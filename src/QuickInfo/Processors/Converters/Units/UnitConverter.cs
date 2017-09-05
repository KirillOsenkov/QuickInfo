using System;
using System.Text;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class UnitConverter : IProcessor
    {
        public string GetResult(Query query)
        {
            var tuple = query.TryGetStructure<Tuple<Double, Unit>>();
            if (tuple != null)
            {
                return GetResult(tuple.Item1.Value, tuple.Item2);
            }

            var list = query.TryGetStructure<SeparatedList>();
            if (list != null && list.SeparatorChar == ' ')
            {
                if (list.Count == 2 || list.Count == 4)
                {
                    var firstNumber = list.TryGetStructure<Double>(0);
                    var unit = list.TryGetStructure<Unit>(1);
                    if (firstNumber != null && unit != null)
                    {
                        if (list.Count == 2)
                        {
                            return GetResult(firstNumber.Value, unit);
                        }

                        var keyword = list.TryGetStructure<Keyword>(2);
                        var tounit = list.TryGetStructure<Unit>(3);
                        if ((keyword == "in" || keyword == "to") && tounit != null)
                        {
                            return GetResult(firstNumber.Value, unit, tounit);
                        }
                    }
                }

                if (list.Count == 3)
                {
                    var firstTuple = list.TryGetStructure<Tuple<Double, Unit>>(0);
                    var keyword = list.TryGetStructure<Keyword>(1);
                    var toUnit = list.TryGetStructure<Unit>(2);
                    if (firstTuple != null &&
                        (keyword == "in" || keyword == "to") &&
                        toUnit != null)
                    {
                        return GetResult(firstTuple.Item1.Value, firstTuple.Item2, toUnit);
                    }
                }

                if (list.Count == 2)
                {
                    var firstTuple = list.TryGetStructure<Tuple<Double, Unit>>(0);
                    var secondTuple = list.TryGetStructure<Tuple<Double, Unit>>(1);
                    if (firstTuple != null &&
                        secondTuple != null &&
                        firstTuple.Item2 == Units.Foot &&
                        secondTuple.Item2 == Units.Inch)
                    {
                        return GetResult(firstTuple.Item1.Value * 12 + secondTuple.Item1.Value, Units.Inch);
                    }

                    var secondNumber = list.TryGetStructure<Double>(1);
                    if (firstTuple != null &&
                        firstTuple.Item2 == Units.Foot &&
                        secondNumber != null)
                    {
                        return GetResult(firstTuple.Item1.Value * 12 + secondNumber.Value, Units.Inch);
                    }
                }

                if (list.Count == 3 || list.Count == 4)
                {
                    var firstNumber = list.TryGetStructure<Double>(0);
                    var unit = list.TryGetStructure<Unit>(1);
                    var secondNumber = list.TryGetStructure<Double>(2);

                    if (list.Count == 3 &&
                        firstNumber != null &&
                        unit == Units.Foot &&
                        secondNumber != null)
                    {
                        return GetResult(firstNumber.Value * 12 + secondNumber.Value, Units.Inch);
                    }

                    if (list.Count == 4 &&
                        firstNumber != null &&
                        unit == Units.Foot &&
                        secondNumber != null &&
                        list.TryGetStructure<Unit>(3) == Units.Inch)
                    {
                        return GetResult(firstNumber.Value * 12 + secondNumber.Value, Units.Inch);
                    }
                }
            }

            return null;
        }

        private string GetResult(double value, Unit unit, Unit toUnit)
        {
            var sb = new StringBuilder();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit && conversion.To == toUnit)
                {
                    var result = GetResult(value, conversion);
                    sb.Append(result);
                }
            }

            if (sb.Length == 0)
            {
                Conversion first = null;
                Conversion second = null;

                // no direct conversion, try 2-step chain
                foreach (var conversion in Units.Conversions)
                {
                    if (conversion.From == unit)
                    {
                        first = conversion;
                    }

                    if (conversion.To == toUnit)
                    {
                        second = conversion;
                    }
                }

                if (first != null && second != null && first.To == second.From)
                {
                    var composite = new Conversion(first.From, second.To, v => second.Converter(first.Converter(v)));
                    sb.Append(GetResult(value, composite));
                }
            }

            return sb.ToString();
        }

        private string GetResult(double value, Unit unit)
        {
            var sb = new StringBuilder();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit)
                {
                    var result = GetResult(value, conversion);
                    sb.Append(result);
                }
            }

            if (sb.Length == 0)
            {
                return null;
            }

            return sb.ToString();
        }

        private string GetResult(double value, Conversion conversion)
        {
            return DivClass($"{value} {conversion.From.ToString()} = {conversion.Converter(value)} {conversion.To.ToString()}", "fixed");
        }
    }
}
