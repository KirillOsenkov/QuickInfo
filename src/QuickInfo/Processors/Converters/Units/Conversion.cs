using System;

namespace QuickInfo
{
    public class Conversion
    {
        public Unit From { get; set; }
        public Unit To { get; set; }
        public Func<double, double> Converter { get; set; }
        public string Format { get; set; }

        public Conversion(Unit from, Unit to, Func<double, double> converter, string format)
        {
            From = from;
            To = to;
            Converter = converter;
            Format = format;
        }

        public Conversion(Unit from, Unit to, Func<double, double> converter) : this(from, to, converter, String.Empty)
        {
        }
    }
}
