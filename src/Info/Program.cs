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
                typeof(HttpStatusCode),
                typeof(QuickInfo.Math),
                typeof(NumberList),
                typeof(RandomGuid),
                typeof(UnitConverter),
                typeof(UrlDecode)
            );
            var query = new Query(commandLine);
            var answers = engine.GetResults(query);
            foreach (var answer in answers)
            {
                ConsoleRenderer.RenderObject(answer.processorName, answer.resultNode);
            }
        }
    }
}
