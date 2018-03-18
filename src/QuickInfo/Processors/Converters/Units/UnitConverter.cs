using System;
using System.Collections.Generic;
using System.Text;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class UnitConverter : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return new Node
                {
                    List = new List<object>
                    {
                        SectionHeader("Temperature"),
                        HelpTable
                        (
                            ("75 f", "")
                        ),
                        SectionHeader("Weight"),
                        HelpTable
                        (
                            ("167 lb", ""),
                            ("30 ounces to grams", "")
                        ),
                        SectionHeader("Distance"),
                        HelpTable
                        (
                            ("26.2 miles", ""),
                            ("900 ft in yards", "")
                        ),
                        SectionHeader("Speed"),
                        HelpTable
                        (
                            ("60 mph", "")
                        ),
                        SectionHeader("Time"),
                        HelpTable
                        (
                            ("127 hours in days", "")
                        ),
                        SectionHeader("Volume"),
                        HelpTable
                        (
                            ("5 gallons in m3", "")
                        ),
                        SectionHeader("Area"),
                        HelpTable
                        (
                            ("1670 sq.ft", ""),
                            ("10 acres in m2", "")
                        ),
                        SectionHeader("Fuel efficiency"),
                        HelpTable
                        (
                            ("29 mpg", "")
                        ),
                        SectionHeader("Currency converter"),
                        HelpTable
                        (
                            ("150 EUR in USD", ""),
                            ("$4000", "")
                        )
                    }
                };
            }

            var plus = query.TryGetStructure<InfixOperator>();
            if (plus != null && plus.OperatorText == "+")
            {
                var left = StructureParser.TryGetStructure<Tuple<Double, Unit>>(plus.Left);
                var right = StructureParser.TryGetStructure<Tuple<Double, Unit>>(plus.Right);
                if (left != null && right != null)
                {
                    if (left.Item2 == right.Item2)
                    {
                        return GetResult(left.Item1.Value + right.Item1.Value, left.Item2);
                    }
                }
            }

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

        private object GetResult(double value, Unit unit, Unit toUnit)
        {
            var list = new List<object>();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit && conversion.To == toUnit)
                {
                    var result = GetResult(value, conversion);
                    list.Add(result);
                }
            }

            if (list.Count == 0)
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
                    list.Add(GetResult(value, composite));
                }
            }

            if (list.Count == 0)
            {
                return null;
            }

            return list;
        }

        private object GetResult(double value, Unit unit)
        {
            var list = new List<object>();

            foreach (var conversion in Units.Conversions)
            {
                if (conversion.From == unit)
                {
                    var result = GetResult(value, conversion);
                    if (result != null)
                    {
                        list.Add(result);
                    }
                }
            }

            if (list.Count == 0)
            {
                return null;
            }

            return new Node
            {
                List = list
            };
        }

        private object GetResult(double value, Conversion conversion)
        {
            return FixedParagraph($"{value} {conversion.From.ToString()} = {conversion.Converter(value).ToString(conversion.Format)} {conversion.To.ToString()}");
        }
    }
}
