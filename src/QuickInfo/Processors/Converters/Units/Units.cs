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
        public static readonly Unit Mile = new Unit("miles", "mile", "mi");
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
        public static readonly Unit FluidOunce = new Unit("fl.oz.", "fl.oz", "fluid ounce", "fluid ounces");
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

        public static readonly Unit Nanosecond = new Unit("ns", "nanoseconds", "nanosecond");
        public static readonly Unit Microsecond = new Unit("μs", "microseconds", "microsecond");
        public static readonly Unit Millisecond = new Unit("ms", "milliseconds", "millisecond");
        public static readonly Unit Second = new Unit("s", "seconds", "second");
        public static readonly Unit Minute = new Unit("m", "minutes", "minute");
        public static readonly Unit Hour = new Unit("h", "hours", "hour");
        public static readonly Unit Day = new Unit("days", "day");
        public static readonly Unit Week = new Unit("weeks", "week");
        public static readonly Unit Month = new Unit("months", "month");
        public static readonly Unit Year = new Unit("years", "year");
        public static readonly Unit Century = new Unit("centuries", "century");
        public static readonly Unit Millennium = new Unit("millennia", "millennium");

        public static readonly Unit EUR = new Unit("EUR", "Euro", "euros", "€");
        public static readonly Unit USD = new Unit("USD", "United States Dollar", "dollars", "$");
        public static readonly Unit RUB = new Unit("RUB", "Russia ruble", "rubles", "₽");
        public static readonly Unit CZK = new Unit("CZK", "Czech Republic Koruna", "Kč");
        public static readonly Unit ILS = new Unit("ILS", "Israeli new shekel", "shekel", "₪");
        public static readonly Unit CAD = new Unit("CAD", "Canadian Dollar");
        public static readonly Unit CHF = new Unit("CHF", "Swiss Franc");
        public static readonly Unit RON = new Unit("RON", "Romanian Leu");
        public static readonly Unit AUD = new Unit("AUD", "Australian Dollar");
        public static readonly Unit PLN = new Unit("PLN", "Polish Zloty");
        public static readonly Unit HUF = new Unit("HUF", "Hungarian Forint");
        public static readonly Unit JPY = new Unit("JPY", "Japanese Yen");
        public static readonly Unit CNY = new Unit("CNY", "Chinese Yuan");
        public static readonly Unit GBP = new Unit("GBP", "Pound Sterling");

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
            new Conversion(FluidOunce, Milliliter, p => p / 0.0338140227),
            new Conversion(Milliliter, FluidOunce, p => p * 0.0338140227),

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

            new Conversion(Nanosecond, Second, p => p / 1000000000.0),
            new Conversion(Second, Nanosecond, p => p * 1000000000.0),
            new Conversion(Microsecond, Second, p => p / 1000000.0),
            new Conversion(Second, Microsecond, p => p * 1000000.0),
            new Conversion(Millisecond, Second, p => p / 1000.0),
            new Conversion(Second, Millisecond, p => p * 1000.0),
            new Conversion(Minute, Second, p => p * 60.0),
            new Conversion(Second, Minute, p => p / 60.0),
            new Conversion(Hour, Second, p => p * 60.0 * 60.0),
            new Conversion(Second, Hour, p => p / 60.0 / 60.0),
            new Conversion(Day, Second, p => p * 60.0 * 60.0 * 24.0),
            new Conversion(Second, Day, p => p / 60.0 / 60.0 / 24.0),
            new Conversion(Week, Second, p => p * 60.0 * 60.0 * 24.0 * 7.0),
            new Conversion(Second, Week, p => p / 60.0 / 60.0 / 24.0 / 7.0),
            new Conversion(Month, Second, p => p * 60.0 * 60.0 * 24.0 * 30.0),
            new Conversion(Second, Month, p => p / 60.0 / 60.0 / 24.0 / 30.0),
            new Conversion(Year, Second, p => p * 60.0 * 60.0 * 24.0 * 365.0),
            new Conversion(Second, Year, p => p / 60.0 / 60.0 / 24.0 / 365.0),
            new Conversion(Century, Second, p => p * 60.0 * 60.0 * 24.0 * 365.0 * 100.0),
            new Conversion(Second, Century, p => p / 60.0 / 60.0 / 24.0 / 365.0 / 100.0),
            new Conversion(Millennium, Second, p => p * 60.0 * 60.0 * 24.0 * 365.0 * 1000.0),
            new Conversion(Second, Millennium, p => p / 60.0 / 60.0 / 24.0 / 365.0 / 1000.0),

            new Conversion(EUR, USD, p => Currency.Convert("EUR", "USD", p), "n2"),
            new Conversion(USD, EUR, p => Currency.Convert("USD", "EUR", p), "n2"),
            new Conversion(EUR, RUB, p => Currency.Convert("EUR", "RUB", p), "n2"),
            new Conversion(RUB, EUR, p => Currency.Convert("RUB", "EUR", p), "n2"),
            new Conversion(EUR, CZK, p => Currency.Convert("EUR", "CZK", p), "n2"),
            new Conversion(CZK, EUR, p => Currency.Convert("CZK", "EUR", p), "n2"),
            new Conversion(EUR, ILS, p => Currency.Convert("EUR", "ILS", p), "n2"),
            new Conversion(ILS, EUR, p => Currency.Convert("ILS", "EUR", p), "n2"),
            new Conversion(EUR, CAD, p => Currency.Convert("EUR", "CAD", p), "n2"),
            new Conversion(CAD, EUR, p => Currency.Convert("CAD", "EUR", p), "n2"),
            new Conversion(EUR, CHF, p => Currency.Convert("EUR", "CHF", p), "n2"),
            new Conversion(CHF, EUR, p => Currency.Convert("CHF", "EUR", p), "n2"),
            new Conversion(RON, EUR, p => Currency.Convert("RON", "EUR", p), "n2"),
            new Conversion(EUR, RON, p => Currency.Convert("EUR", "RON", p), "n2"),
            new Conversion(USD, RON, p => Currency.Convert("USD", "RON", p), "n2"),
            new Conversion(RON, USD, p => Currency.Convert("RON", "USD", p), "n2"),
            new Conversion(AUD, EUR, p => Currency.Convert("AUD", "EUR", p), "n2"),
            new Conversion(EUR, AUD, p => Currency.Convert("EUR", "AUD", p), "n2"),
            new Conversion(USD, AUD, p => Currency.Convert("USD", "AUD", p), "n2"),
            new Conversion(AUD, USD, p => Currency.Convert("AUD", "USD", p), "n2"),
            new Conversion(PLN, EUR, p => Currency.Convert("PLN", "EUR", p), "n2"),
            new Conversion(EUR, PLN, p => Currency.Convert("EUR", "PLN", p), "n2"),
            new Conversion(USD, PLN, p => Currency.Convert("USD", "PLN", p), "n2"),
            new Conversion(PLN, USD, p => Currency.Convert("PLN", "USD", p), "n2"),
            new Conversion(HUF, EUR, p => Currency.Convert("HUF", "EUR", p), "n2"),
            new Conversion(EUR, HUF, p => Currency.Convert("EUR", "HUF", p), "n2"),
            new Conversion(USD, HUF, p => Currency.Convert("USD", "HUF", p), "n2"),
            new Conversion(HUF, USD, p => Currency.Convert("HUF", "USD", p), "n2"),
            new Conversion(JPY, EUR, p => Currency.Convert("JPY", "EUR", p), "n2"),
            new Conversion(EUR, JPY, p => Currency.Convert("EUR", "JPY", p), "n2"),
            new Conversion(USD, JPY, p => Currency.Convert("USD", "JPY", p), "n2"),
            new Conversion(JPY, USD, p => Currency.Convert("JPY", "USD", p), "n2"),
            new Conversion(CNY, EUR, p => Currency.Convert("CNY", "EUR", p), "n2"),
            new Conversion(EUR, CNY, p => Currency.Convert("EUR", "CNY", p), "n2"),
            new Conversion(USD, CNY, p => Currency.Convert("USD", "CNY", p), "n2"),
            new Conversion(CNY, USD, p => Currency.Convert("CNY", "USD", p), "n2"),
            new Conversion(GBP, EUR, p => Currency.Convert("GBP", "EUR", p), "n2"),
            new Conversion(EUR, GBP, p => Currency.Convert("EUR", "GBP", p), "n2"),
            new Conversion(USD, GBP, p => Currency.Convert("USD", "GBP", p), "n2"),
            new Conversion(GBP, USD, p => Currency.Convert("GBP", "USD", p), "n2"),
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
