using System;
using GuiLabs.MathParser;
using static QuickInfo.NodeFactory;

namespace QuickInfo
{
    public class Math : IProcessor
    {
        private static readonly Random random = new Random();

        public static double Random(double lowerBound, double upperBound)
        {
            return lowerBound + Random() * (upperBound - lowerBound);
        }

        public static double Rnd(double lowerBound, double upperBound)
        {
            return Random(lowerBound, upperBound);
        }

        public static double Random(double upperBound)
        {
            return Random() * upperBound;
        }

        public static double Rnd(double upperBound)
        {
            return Random(upperBound);
        }

        public static double Random()
        {
            return random.NextDouble();
        }

        public static double Rnd()
        {
            return Random();
        }

        static Math()
        {
            Binder.Default.RegisterStaticMethods<Math>();
        }

        public object GetResult(Query query)
        {
            if (query.IsHelp)
            {
                return HelpTable(
                    ("(2 + 3) * 4", ""),
                    ("cos(pi / 4)", ""),
                    ("rnd", "Random number [0; 1)"),
                    ("Random(50, 100)", "Random number [50; 100)"));
            }

            var compiler = new Compiler();
            var result = compiler.CompileExpression(query.OriginalInput);
            if (result.IsSuccess)
            {
                double output;
                try
                {
                    output = result.Expression();
                }
                catch (Exception ex)
                {
                    return FixedParagraph(ex.ToString());
                }

                if (output.ToString() == query.OriginalInput.Trim())
                {
                    // tautology
                    return null;
                }

                return FixedParagraph($"{output}");
            }

            return null;
        }
    }
}
