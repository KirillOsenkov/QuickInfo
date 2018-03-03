using System;
using QuickInfo;

namespace Info
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLine = string.Join(" ", args);
            var engine = new Engine();
            var query = new Query(commandLine);
            var answers = engine.GetResults(query);
            foreach (var answer in answers)
            {
                ConsoleRenderer.RenderObject(answer.processorName, answer.resultNode);
            }
        }
    }
}
