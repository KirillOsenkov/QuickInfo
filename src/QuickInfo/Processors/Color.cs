using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static System.Math;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Color : IProcessor
    {
        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable
                (
                    ("color", "Named color palette"),
                    ("red", "Named color"),
                    ("#FFC0CB", "Hex color"),
                    ("rgb 23 145 175", "RGB"),
                    ("rgb(rnd*255, rnd*255, rnd*255)", "Random RGB color")
                );
            }

            var input = query.OriginalInputTrim;

            if (string.Equals(input, "color", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(input, "colors", StringComparison.OrdinalIgnoreCase))
            {
                return RenderColorTable();
            }

            string knownColor = null;
            if (knownColors.TryGetValue(input.Trim(), out knownColor))
            {
                return GetResultFromHexString(knownColor);
            }

            if (query.Structure == null)
            {
                return null;
            }

            SeparatedList list = query.TryGetStructure<SeparatedList>();

            if (list == null)
            {
                var invocation = query.TryGetStructure<Invocation>();
                if (invocation != null &&
                    string.Equals(invocation.Prefix, "rgb", StringComparison.OrdinalIgnoreCase))
                {
                    list = StructureParser.TryGetStructure<SeparatedList>(invocation.ArgumentListParsed);
                    if (list != null && list.Count != 3)
                    {
                        list = null;
                    }
                }
            }

            if (list != null &&
                (list.Count == 3 ||
                (list.Count == 4 &&
                    (list.TryGetStructure<Keyword>(0) == "rgb" ||
                     list.TryGetStructure<Keyword>(3) == "rgb"))))
            {
                var intList = list.GetStructuresOfType<Integer>();
                if (intList.Count == 3)
                {
                    var r = intList[0].Value;
                    var g = intList[1].Value;
                    var b = intList[2].Value;
                    if (r >= 0 && r < 256 && g >= 0 && g < 256 && b >= 0 && b < 256)
                    {
                        return GetResult((int)r, (int)g, (int)b);
                    }
                }
            }

            var hashPrefix = query.TryGetStructure<Prefix>();
            if (hashPrefix != null &&
                (hashPrefix.PrefixString == "#" || hashPrefix.PrefixString == "0x"))
            {
                var remainderString = hashPrefix.RemainderString;
                if (remainderString.Length == 3 || remainderString.Length == 6 || remainderString.Length == 8)
                {
                    var integer = StructureParser.TryGetStructure<Integer>(hashPrefix.Remainder);
                    if (integer != null)
                    {
                        return GetResultFromHexString(remainderString);
                    }
                }
            }

            return null;
        }

        private object RenderColorTable()
        {
            var swatches = new string[10, 14]
            {
                { "Black", "DarkSlateGray", "DarkGreen", "Green", "Olive", "DarkOliveGreen", "SeaGreen", "DarkSeaGreen", "LimeGreen", "YellowGreen", "MediumAquamarine", "LightSeaGreen", "DarkCyan", "Teal" },
                { "DimGray", "SlateGray", "LightSlateGray", "ForestGreen", "DarkKhaki", "OliveDrab", "MediumSeaGreen", "Chartreuse", "LightGreen", "MediumSpringGreen", "Aquamarine", "Turquoise", "MediumTurquoise", "CadetBlue" },
                { "Gray", "DarkGray", "Gainsboro", "LightGoldenrodYellow", "Beige", "GreenYellow", "LawnGreen", "Lime", "SpringGreen", "Honeydew", "PaleTurquoise", "LightBlue", "DarkTurquoise", "SteelBlue" },
                { "Silver", "LightGray", "WhiteSmoke", "Snow", "White", "LemonChiffon", "PaleGreen", "Ivory", "FloralWhite", "MintCream", "LightCyan", "Cyan", "DeepSkyBlue", "CornflowerBlue" },
                { "Khaki", "BlanchedAlmond", "Bisque", "Cornsilk", "Transparent", "OldLace", "White", "Azure", "GhostWhite", "AliceBlue", "PowderBlue", "LightSkyBlue", "DodgerBlue", "MediumBlue" },
                { "PaleGoldenrod", "Wheat", "Moccasin", "LightYellow", "PapayaWhip", "PeachPuff", "AntiqueWhite", "SeaShell", "LavenderBlush", "Lavender", "LightSteelBlue", "SkyBlue", "RoyalBlue", "Blue" },
                { "Yellow", "BurlyWood", "NavajoWhite", "Orange", "Coral", "DarkSalmon", "LightSalmon", "LightPink", "MistyRose", "Linen", "Thistle", "MediumSlateBlue", "SlateBlue", "DarkSlateBlue" },
                { "Gold", "Tan", "SandyBrown", "DarkOrange", "Tomato", "Salmon", "LightCoral", "RosyBrown", "Violet", "Pink", "Plum", "BlueViolet", "MediumPurple", "DarkBlue" },
                { "Goldenrod", "Peru", "Chocolate", "OrangeRed", "Firebrick", "Crimson", "IndianRed", "PaleVioletRed", "Orchid", "MediumOrchid", "DarkViolet", "DarkOrchid", "Indigo", "Navy" },
                { "DarkGoldenrod", "SaddleBrown", "Sienna", "Red", "DarkRed", "Maroon", "Brown", "HotPink", "Magenta", "DeepPink", "MediumVioletRed", "DarkMagenta", "Purple", "MidnightBlue" },
            }.AsJagged();

            return new Node
            {
                Kind = "Table",
                Style = "Color",
                List = swatches.Select(row => new Node
                {
                    Kind = "Row",
                    List = row.Select(swatch => new Node
                    {
                        Kind = "Cell",
                        Style = "Color",
                        Text = swatch
                    })
                })
            };
        }

        private object GetResultFromHexString(string hexString)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            int a = 0;

            if (hexString.Length == 3)
            {
                r = int.Parse(new string(hexString[0], 2), NumberStyles.AllowHexSpecifier);
                g = int.Parse(new string(hexString[1], 2), NumberStyles.AllowHexSpecifier);
                b = int.Parse(new string(hexString[2], 2), NumberStyles.AllowHexSpecifier);
            }
            else if (hexString.Length == 6)
            {
                r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
            }
            else if (hexString.Length == 8)
            {
                a = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                r = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                g = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                b = int.Parse(hexString.Substring(6, 2), NumberStyles.AllowHexSpecifier);
            }

            return GetResult(r, g, b);
        }

        private object GetResult(int r, int g, int b)
        {
            var hexColor = GetHexColor(r, g, b);
            var result = new List<object>();

            result.Add(FixedParagraph($"RGB({r},{g},{b}) = {hexColor}"));

            result.Add(new Node
            {
                Text = hexColor,
                Style = "ColorSwatchLarge"
            });

            string knownColor = null;
            if (knownColorNames.TryGetValue(r + g * 256 + b * 65536, out knownColor))
            {
                var swatchName = Paragraph(knownColor);
                swatchName.Style = "ColorSwatchName";
                result.Add(swatchName);
            }

            var nearestColors = GetNearestColors(r, g, b).Take(11);

            result.Add(SectionHeader("Closest named colors:"));

            var table = new Node
            {
                Kind = "Table",
                List = nearestColors.Where(n => knownColor != n).Select(nearestColor =>
                {
                    var hex = "#" + knownColors[nearestColor];
                    return new Node
                    {
                        Kind = "Row",
                        List = new Node[]
                        {
                            new Node
                            {
                                Kind = "Cell",
                                Text = nearestColor
                            },
                            new Node
                            {
                                Kind = "Cell",
                                Text = hex,
                                SearchLink = hex
                            },
                            new Node
                            {
                                Kind = "Cell",
                                Style = "ColorSwatchSmall",
                                Text = hex,
                                SearchLink = nearestColor
                            }
                        }
                    };
                })
            };
            result.Add(table);

            return result;
        }

        private IEnumerable<string> GetNearestColors(int r, int g, int b)
        {
            var colorMapByDistance = new List<Tuple<string, double>>();

            foreach (var kvp in knownColorNames)
            {
                int knownColor = kvp.Key;
                int knownR = knownColor % 256;
                int knownG = (knownColor >> 8) % 256;
                int knownB = (knownColor >> 16) % 256;
                double distance = Distance(r, g, b, knownR, knownG, knownB);
                colorMapByDistance.Add(Tuple.Create(kvp.Value, distance));
            }

            colorMapByDistance.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));
            return colorMapByDistance.Select(t => t.Item1);
        }

        private double Distance(int r1, int g1, int b1, int r2, int g2, int b2)
        {
            double dr = (r1 - r2) / 255.0;
            double dg = (g1 - g2) / 255.0;
            double db = (b1 - b2) / 255.0;
            return Sqrt(dr * dr * 0.299 + dg * dg * 0.587 + db * db * 0.114);
        }

        private string GetHexColor(int r, int g, int b)
        {
            string rhex = r.ToHexByte();
            string ghex = g.ToHexByte();
            string bhex = b.ToHexByte();
            if (rhex[0] == rhex[1] && ghex[0] == ghex[1] && bhex[0] == bhex[1])
            {
                rhex = rhex.Substring(1);
                ghex = ghex.Substring(1);
                bhex = bhex.Substring(1);
            }

            return "#" + rhex + ghex + bhex;
        }

        private static readonly Dictionary<int, string> knownColorNames = new Dictionary<int, string>()
        {
            { 16775408, "AliceBlue" },
            { 14150650, "AntiqueWhite" },
            { 13959039, "Aquamarine" },
            { 16777200, "Azure" },
            { 14480885, "Beige" },
            { 12903679, "Bisque" },
            { 0, "Black" },
            { 13495295, "BlanchedAlmond" },
            { 16711680, "Blue" },
            { 14822282, "BlueViolet" },
            { 2763429, "Brown" },
            { 8894686, "BurlyWood" },
            { 10526303, "CadetBlue" },
            { 65407, "Chartreuse" },
            { 1993170, "Chocolate" },
            { 5275647, "Coral" },
            { 15570276, "CornflowerBlue" },
            { 14481663, "Cornsilk" },
            { 3937500, "Crimson" },
            { 16776960, "Cyan" },
            { 9109504, "DarkBlue" },
            { 9145088, "DarkCyan" },
            { 755384, "DarkGoldenrod" },
            { 11119017, "DarkGray" },
            { 25600, "DarkGreen" },
            { 7059389, "DarkKhaki" },
            { 9109643, "DarkMagenta" },
            { 3107669, "DarkOliveGreen" },
            { 36095, "DarkOrange" },
            { 13382297, "DarkOrchid" },
            { 139, "DarkRed" },
            { 8034025, "DarkSalmon" },
            { 9419919, "DarkSeaGreen" },
            { 9125192, "DarkSlateBlue" },
            { 5197615, "DarkSlateGray" },
            { 13749760, "DarkTurquoise" },
            { 13828244, "DarkViolet" },
            { 9639167, "DeepPink" },
            { 16760576, "DeepSkyBlue" },
            { 6908265, "DimGray" },
            { 16748574, "DodgerBlue" },
            { 2237106, "Firebrick" },
            { 15792895, "FloralWhite" },
            { 2263842, "ForestGreen" },
            { 14474460, "Gainsboro" },
            { 16775416, "GhostWhite" },
            { 55295, "Gold" },
            { 2139610, "Goldenrod" },
            { 8421504, "Gray" },
            { 32768, "Green" },
            { 3145645, "GreenYellow" },
            { 15794160, "Honeydew" },
            { 11823615, "HotPink" },
            { 6053069, "IndianRed" },
            { 8519755, "Indigo" },
            { 15794175, "Ivory" },
            { 9234160, "Khaki" },
            { 16443110, "Lavender" },
            { 16118015, "LavenderBlush" },
            { 64636, "LawnGreen" },
            { 13499135, "LemonChiffon" },
            { 15128749, "LightBlue" },
            { 8421616, "LightCoral" },
            { 16777184, "LightCyan" },
            { 13826810, "LightGoldenrodYellow" },
            { 13882323, "LightGray" },
            { 9498256, "LightGreen" },
            { 12695295, "LightPink" },
            { 8036607, "LightSalmon" },
            { 11186720, "LightSeaGreen" },
            { 16436871, "LightSkyBlue" },
            { 10061943, "LightSlateGray" },
            { 14599344, "LightSteelBlue" },
            { 14745599, "LightYellow" },
            { 65280, "Lime" },
            { 3329330, "LimeGreen" },
            { 15134970, "Linen" },
            { 16711935, "Magenta" },
            { 128, "Maroon" },
            { 11193702, "MediumAquamarine" },
            { 13434880, "MediumBlue" },
            { 13850042, "MediumOrchid" },
            { 14381203, "MediumPurple" },
            { 7451452, "MediumSeaGreen" },
            { 15624315, "MediumSlateBlue" },
            { 10156544, "MediumSpringGreen" },
            { 13422920, "MediumTurquoise" },
            { 8721863, "MediumVioletRed" },
            { 7346457, "MidnightBlue" },
            { 16449525, "MintCream" },
            { 14804223, "MistyRose" },
            { 11920639, "Moccasin" },
            { 11394815, "NavajoWhite" },
            { 8388608, "Navy" },
            { 15136253, "OldLace" },
            { 32896, "Olive" },
            { 2330219, "OliveDrab" },
            { 42495, "Orange" },
            { 17919, "OrangeRed" },
            { 14053594, "Orchid" },
            { 11200750, "PaleGoldenrod" },
            { 10025880, "PaleGreen" },
            { 15658671, "PaleTurquoise" },
            { 9662683, "PaleVioletRed" },
            { 14020607, "PapayaWhip" },
            { 12180223, "PeachPuff" },
            { 4163021, "Peru" },
            { 13353215, "Pink" },
            { 14524637, "Plum" },
            { 15130800, "PowderBlue" },
            { 8388736, "Purple" },
            { 10040166, "RebeccaPurple" },
            { 255, "Red" },
            { 9408444, "RosyBrown" },
            { 14772545, "RoyalBlue" },
            { 1262987, "SaddleBrown" },
            { 7504122, "Salmon" },
            { 6333684, "SandyBrown" },
            { 5737262, "SeaGreen" },
            { 15660543, "SeaShell" },
            { 2970272, "Sienna" },
            { 12632256, "Silver" },
            { 15453831, "SkyBlue" },
            { 13458026, "SlateBlue" },
            { 9470064, "SlateGray" },
            { 16448255, "Snow" },
            { 8388352, "SpringGreen" },
            { 11829830, "SteelBlue" },
            { 9221330, "Tan" },
            { 8421376, "Teal" },
            { 14204888, "Thistle" },
            { 4678655, "Tomato" },
            { 13688896, "Turquoise" },
            { 15631086, "Violet" },
            { 11788021, "Wheat" },
            { 16777215, "White" },
            { 16119285, "WhiteSmoke" },
            { 65535, "Yellow" },
            { 3329434, "YellowGreen" },
        };

        private static readonly Dictionary<string, string> knownColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "AliceBlue", "F0F8FF" },
            { "AntiqueWhite", "FAEBD7" },
            { "Aqua", "00FFFF" },
            { "Aquamarine", "7FFFD4" },
            { "Azure", "F0FFFF" },
            { "Beige", "F5F5DC" },
            { "Bisque", "FFE4C4" },
            { "Black", "000000" },
            { "BlanchedAlmond", "FFEBCD" },
            { "Blue", "0000FF" },
            { "BlueViolet", "8A2BE2" },
            { "Brown", "A52A2A" },
            { "BurlyWood", "DEB887" },
            { "CadetBlue", "5F9EA0" },
            { "Chartreuse", "7FFF00" },
            { "Chocolate", "D2691E" },
            { "Coral", "FF7F50" },
            { "CornflowerBlue", "6495ED" },
            { "Cornsilk", "FFF8DC" },
            { "Crimson", "DC143C" },
            { "Cyan", "00FFFF" },
            { "DarkBlue", "00008B" },
            { "DarkCyan", "008B8B" },
            { "DarkGoldenrod", "B8860B" },
            { "DarkGray", "A9A9A9" },
            { "DarkGreen", "006400" },
            { "DarkKhaki", "BDB76B" },
            { "DarkMagenta", "8B008B" },
            { "DarkOliveGreen", "556B2F" },
            { "DarkOrange", "FF8C00" },
            { "DarkOrchid", "9932CC" },
            { "DarkRed", "8B0000" },
            { "DarkSalmon", "E9967A" },
            { "DarkSeaGreen", "8FBC8F" },
            { "DarkSlateBlue", "483D8B" },
            { "DarkSlateGray", "2F4F4F" },
            { "DarkTurquoise", "00CED1" },
            { "DarkViolet", "9400D3" },
            { "DeepPink", "FF1493" },
            { "DeepSkyBlue", "00BFFF" },
            { "DimGray", "696969" },
            { "DodgerBlue", "1E90FF" },
            { "Firebrick", "B22222" },
            { "FloralWhite", "FFFAF0" },
            { "ForestGreen", "228B22" },
            { "Fuchsia", "FF00FF" },
            { "Gainsboro", "DCDCDC" },
            { "GhostWhite", "F8F8FF" },
            { "Gold", "FFD700" },
            { "Goldenrod", "DAA520" },
            { "Gray", "808080" },
            { "Green", "008000" },
            { "GreenYellow", "ADFF2F" },
            { "Honeydew", "F0FFF0" },
            { "HotPink", "FF69B4" },
            { "IndianRed", "CD5C5C" },
            { "Indigo", "4B0082" },
            { "Ivory", "FFFFF0" },
            { "Khaki", "F0E68C" },
            { "Lavender", "E6E6FA" },
            { "LavenderBlush", "FFF0F5" },
            { "LawnGreen", "7CFC00" },
            { "LemonChiffon", "FFFACD" },
            { "LightBlue", "ADD8E6" },
            { "LightCoral", "F08080" },
            { "LightCyan", "E0FFFF" },
            { "LightGoldenrodYellow", "FAFAD2" },
            { "LightGreen", "90EE90" },
            { "LightGray", "D3D3D3" },
            { "LightPink", "FFB6C1" },
            { "LightSalmon", "FFA07A" },
            { "LightSeaGreen", "20B2AA" },
            { "LightSkyBlue", "87CEFA" },
            { "LightSlateGray", "778899" },
            { "LightSteelBlue", "B0C4DE" },
            { "LightYellow", "FFFFE0" },
            { "Lime", "00FF00" },
            { "LimeGreen", "32CD32" },
            { "Linen", "FAF0E6" },
            { "Magenta", "FF00FF" },
            { "Maroon", "800000" },
            { "MediumAquamarine", "66CDAA" },
            { "MediumBlue", "0000CD" },
            { "MediumOrchid", "BA55D3" },
            { "MediumPurple", "9370DB" },
            { "MediumSeaGreen", "3CB371" },
            { "MediumSlateBlue", "7B68EE" },
            { "MediumSpringGreen", "00FA9A" },
            { "MediumTurquoise", "48D1CC" },
            { "MediumVioletRed", "C71585" },
            { "MidnightBlue", "191970" },
            { "MintCream", "F5FFFA" },
            { "MistyRose", "FFE4E1" },
            { "Moccasin", "FFE4B5" },
            { "NavajoWhite", "FFDEAD" },
            { "Navy", "000080" },
            { "OldLace", "FDF5E6" },
            { "Olive", "808000" },
            { "OliveDrab", "6B8E23" },
            { "Orange", "FFA500" },
            { "OrangeRed", "FF4500" },
            { "Orchid", "DA70D6" },
            { "PaleGoldenrod", "EEE8AA" },
            { "PaleGreen", "98FB98" },
            { "PaleTurquoise", "AFEEEE" },
            { "PaleVioletRed", "DB7093" },
            { "PapayaWhip", "FFEFD5" },
            { "PeachPuff", "FFDAB9" },
            { "Peru", "CD853F" },
            { "Pink", "FFC0CB" },
            { "Plum", "DDA0DD" },
            { "PowderBlue", "B0E0E6" },
            { "Purple", "800080" },
            { "RebeccaPurple", "663399" },
            { "Red", "FF0000" },
            { "RosyBrown", "BC8F8F" },
            { "RoyalBlue", "4169E1" },
            { "SaddleBrown", "8B4513" },
            { "Salmon", "FA8072" },
            { "SandyBrown", "F4A460" },
            { "SeaGreen", "2E8B57" },
            { "SeaShell", "FFF5EE" },
            { "Sienna", "A0522D" },
            { "Silver", "C0C0C0" },
            { "SkyBlue", "87CEEB" },
            { "SlateBlue", "6A5ACD" },
            { "SlateGray", "708090" },
            { "Snow", "FFFAFA" },
            { "SpringGreen", "00FF7F" },
            { "SteelBlue", "4682B4" },
            { "Tan", "D2B48C" },
            { "Teal", "008080" },
            { "Thistle", "D8BFD8" },
            { "Tomato", "FF6347" },
            { "Turquoise", "40E0D0" },
            { "Violet", "EE82EE" },
            { "Wheat", "F5DEB3" },
            { "White", "FFFFFF" },
            { "WhiteSmoke", "F5F5F5" },
            { "Yellow", "FFFF00" },
            { "YellowGreen", "9ACD32" },
        };
    }
}
