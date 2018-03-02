using System.Linq;

namespace QuickInfo
{
    public class Units
    {
        public static readonly Unit Pound = new Unit("lb", "lbs", "pound", "pounds");
        public static readonly Unit Ounce = new Unit("oz", "ounce", "ounces");
        public static readonly Unit Gram = new Unit("gr", "gram", "grams");
        public static readonly Unit Kilogram = new Unit("kg", "kilo", "kilogram", "kilograms");
        public static readonly Unit Ton = new Unit("t", "ton", "tons");

        public static readonly Unit Kilometer = new Unit("km", "kilometer", "kilometers", "kilometre", "kilometres");
        public static readonly Unit Centimeter = new Unit("cm", "centimeter", "centimetre", "centimeters", "centimetres");
        public static readonly Unit Millimeter = new Unit("mm", "millimeter", "millimetre", "millimeters", "millimetres");
        public static readonly Unit Inch = new Unit("in", "inch", "inches", "\"");
        public static readonly Unit Foot = new Unit("ft", "foot", "feet", "'");
        public static readonly Unit Mile = new Unit("miles", "mile");
        public static readonly Unit Yard = new Unit("yd", "yard", "yards");
        public static readonly Unit Meter = new Unit("m", "meter", "metre", "meters", "metres");

        public static readonly Unit Mph = new Unit("mph", "mile/h", "miles/h", "mile/hour", "miles/hour");
        public static readonly Unit Kmh = new Unit("kmh", "km/h", "km/hour", "kilometer/hour", "kilometers/hour");
        public static readonly Unit FtSecond = new Unit("f/s", "fps", "ft/s", "foot/s", "foot/second", "feet/s", "feet/second");
        public static readonly Unit Mps = new Unit("m/s", "mps", "meters/second");
        public static readonly Unit Knot = new Unit("knot", "knots");

        public static readonly Unit Fahrenheit = new Unit("f", "fahrenheit", "fahrenheits");
        public static readonly Unit Celsius = new Unit("c", "celsius");
        public static readonly Unit Kelvin = new Unit("k", "kelvin", "kelvins");

        public static readonly Unit Gallon = new Unit("gallon", "gallons");
        public static readonly Unit Quart = new Unit("quart", "quarts");
        public static readonly Unit Pint = new Unit("pint");
        public static readonly Unit FluidOunce = new Unit("fl.oz.", "fl.oz");
        public static readonly Unit Liter = new Unit("l", "liter", "liters", "litre", "litres");
        public static readonly Unit Milliliter = new Unit("ml", "milliliter", "milliliters", "millilitre", "millilitres");
        public static readonly Unit CubicMeter = new Unit("m³", "m3");
        public static readonly Unit CubicFoot = new Unit("ft³", "ft3");
        public static readonly Unit CubicInch = new Unit("in³", "in3");

        public static readonly Unit SquareMeter = new Unit("m²", "sq.m", "sq.m.", "m2");
        public static readonly Unit SquareFoot = new Unit("sq.ft", "sq.ft.", "ft²", "sqft", "ft2");
        public static readonly Unit SquareInch = new Unit("sq.in", "sq.in.", "in²", "in2");
        public static readonly Unit SquareYard = new Unit("sq.yd", "sq.yd.", "yd²", "yd2");
        public static readonly Unit SquareMile = new Unit("sq.miles", "sq.mile", "mile²", "mile2");
        public static readonly Unit SquareKilometer = new Unit("km²", "sq.km", "sq.km.", "km2");
        public static readonly Unit Hectare = new Unit("hectares", "hectare", "hectar", "hectars");
        public static readonly Unit Acre = new Unit("acres", "acre");

        public static readonly Unit Mpg = new Unit("mpg", "miles/gallon");
        public static readonly Unit LitersPer100Km = new Unit("liters/100km", "l/100km");

        public static readonly Unit EUR = new Unit("EUR", "Euro", "€");
        public static readonly Unit USD = new Unit("USD", "United States Dollar", "$");
        public static readonly Unit RUB = new Unit("RUB", "Russia ruble", "₽");
        public static readonly Unit CZK = new Unit("CZK", "Czech Republic Koruna", "Kč");

        public static readonly Conversion[] Conversions =
        {
            new Conversion(Pound, Kilogram, p => p * 0.45359237),
            new Conversion(Kilogram, Pound, p => p / 0.45359237),
            new Conversion(Ounce, Kilogram, p => p * 0.028349523125),
            new Conversion(Kilogram, Ounce, p => p / 0.028349523125),
            new Conversion(Gram, Kilogram, p => p / 1000),
            new Conversion(Kilogram, Gram, p => p * 1000),
            new Conversion(Ton, Kilogram, p => p * 1000),
            new Conversion(Kilogram, Ton, p => p / 1000),

            new Conversion(Mile, Meter, p => p * 1609.344),
            new Conversion(Meter, Mile, p => p / 1609.344),
            new Conversion(Foot, Meter, p => p * 0.3048),
            new Conversion(Meter, Foot, p => p / 0.3048),
            new Conversion(Inch, Meter, p => p * 0.0254),
            new Conversion(Meter, Inch, p => p / 0.0254),
            new Conversion(Yard, Meter, p => p * 0.9144),
            new Conversion(Meter, Yard, p => p / 0.9144),
            new Conversion(Kilometer, Meter, p => p * 1000),
            new Conversion(Meter, Kilometer, p => p / 1000),
            new Conversion(Centimeter, Meter, p => p / 100),
            new Conversion(Meter, Centimeter, p => p * 100),
            new Conversion(Millimeter, Meter, p => p / 1000),
            new Conversion(Meter, Millimeter, p => p * 1000),

            new Conversion(Mph, Kmh, p => p * 1.609344),
            new Conversion(Kmh, Mph, p => p / 1.609344),
            new Conversion(FtSecond, Kmh, p => p * 1.09728),
            new Conversion(Kmh, FtSecond, p => p / 1.09728),
            new Conversion(Mps, Kmh, p => p * 3.6),
            new Conversion(Kmh, Mps, p => p / 3.6),
            new Conversion(Knot, Kmh, p => p * 1.852),
            new Conversion(Kmh, Knot, p => p / 1.852),

            new Conversion(Fahrenheit, Celsius, p => (p - 32) * 5 / 9, "f2"),
            new Conversion(Celsius, Fahrenheit, p => p * 9 / 5 + 32, "f2"),
            new Conversion(Kelvin, Celsius, p => p - 273.15, "f2"),
            new Conversion(Celsius, Kelvin, p => p + 273.15, "f2"),

            new Conversion(Gallon, Liter, p => p * 3.78541178),
            new Conversion(Liter, Gallon, p => p / 3.78541178),
            new Conversion(Quart, Liter, p => p * 0.94635295),
            new Conversion(Liter, Quart, p => p / 0.94635295),
            new Conversion(Pint, Liter, p => p * 0.47317647),
            new Conversion(Liter, Pint, p => p / 0.47317647),
            new Conversion(FluidOunce, Liter, p => p / 33.8140227),
            new Conversion(Liter, FluidOunce, p => p * 33.8140227),
            new Conversion(Milliliter, Liter, p => p / 1000),
            new Conversion(Liter, Milliliter, p => p * 1000),
            new Conversion(CubicMeter, Liter, p => p * 1000),
            new Conversion(Liter, CubicMeter, p => p / 1000),
            new Conversion(CubicFoot, Liter, p => p * 28.3168466),
            new Conversion(Liter, CubicFoot, p => p / 28.3168466),
            new Conversion(CubicInch, Liter, p => p * 0.01638706),
            new Conversion(Liter, CubicInch, p => p / 0.01638706),

            new Conversion(SquareKilometer, SquareMeter, p => p * 1000000),
            new Conversion(SquareMeter, SquareKilometer, p => p / 1000000),
            new Conversion(Hectare, SquareMeter, p => p * 10000),
            new Conversion(SquareMeter, Hectare, p => p / 10000),
            new Conversion(SquareMile, SquareMeter, p => p * 2589988.110336),
            new Conversion(SquareMeter, SquareMile, p => p / 2589988.110336),
            new Conversion(Acre, SquareMeter, p => p * 4046.8564224),
            new Conversion(SquareMeter, Acre, p => p / 4046.8564224),
            new Conversion(SquareYard, SquareMeter, p => p * 0.83612736),
            new Conversion(SquareMeter, SquareYard, p => p / 0.83612736),
            new Conversion(SquareFoot, SquareMeter, p => p * 0.09290304),
            new Conversion(SquareMeter, SquareFoot, p => p / 0.09290304),
            new Conversion(SquareInch, SquareMeter, p => p * 0.00064516),
            new Conversion(SquareMeter, SquareInch, p => p / 0.00064516),

            new Conversion(Mpg, LitersPer100Km, p => 235.214583084785 / p),
            new Conversion(LitersPer100Km, Mpg, p => 235.214583084785 / p),

            new Conversion(EUR, USD, p => Currency.Convert("EUR", "USD", p), "n2"),
            new Conversion(USD, EUR, p => Currency.Convert("USD", "EUR", p), "n2"),
            new Conversion(EUR, RUB, p => Currency.Convert("EUR", "RUB", p), "n2"),
            new Conversion(RUB, EUR, p => Currency.Convert("RUB", "EUR", p), "n2"),
            new Conversion(EUR, CZK, p => Currency.Convert("EUR", "CZK", p), "n2"),
            new Conversion(CZK, EUR, p => Currency.Convert("CZK", "EUR", p), "n2"),
        };

        private static Unit[] allUnits = null;
        public static Unit[] AllUnits
        {
            get
            {
                if (allUnits == null)
                {
                    allUnits = typeof(Units)
                        .GetFields()
                        .Select(f => f.GetValue(null) as Unit)
                        .Where(v => v != null)
                        .ToArray();
                }

                return allUnits;
            }
        }
    }
}
