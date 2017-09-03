using System;
using GuiLabs.MathParser;
using static QuickInfo.HtmlFactory;

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

        public string GetResult(Query query)
        {
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
                    return Div(ex.ToString());
                }

                if (output.ToString() == query.OriginalInput.Trim())
                {
                    // tautology
                    return null;
                }

                return Div(Escape($"{output}"));
            }

            return null;
        }
    }
}
