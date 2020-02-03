using System;
using QuickInfo;

namespace Info
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = string.Join(" ", args);
            var engine = new Engine
            (
                typeof(Ascii),
                typeof(Color),
                typeof(QuickInfo.DateTime),
                typeof(Emoticons),
                typeof(Factor),
                typeof(Hex),
                typeof(Roman),
                typeof(HttpStatusCode),
                typeof(QuickInfo.Math),
                typeof(NumberList),
                typeof(RandomGuid),
                typeof(Unicode),
                typeof(UnitConverter),
                typeof(UrlDecode)
            );
            var query = new Query(commandLine);
            var answers = engine.GetResults(query);
            bool first = true;
            foreach (var answer in answers)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.WriteLine();
                }

                ConsoleRenderer.RenderObject(answer.processorName, answer.resultNode);
            }
        }
    }
}
