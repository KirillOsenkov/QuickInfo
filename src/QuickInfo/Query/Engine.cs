using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;
using static QuickInfo.HtmlFactory;

namespace QuickInfo
{
    public class Engine
    {
        private List<IProcessor> processors = new List<IProcessor>();
        private List<IStructureParser> structureParsers = new List<IStructureParser>();

        private Engine()
        {
            var assembly = typeof(Engine).GetTypeInfo().Assembly;
            var processorTypes = assembly.GetTypes()
                .Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IProcessor)));

            processors.AddRange(processorTypes.Select(t => (IProcessor)Activator.CreateInstance(t)));

            structureParsers.Add(new UnitParser());
            structureParsers.Add(new Keyword("rgb"));
            structureParsers.Add(new Keyword("in"));
            structureParsers.Add(new Keyword("to"));
            structureParsers.Add(new Keyword("hex"));
            structureParsers.Add(new Invocation());
            structureParsers.Add(new Prefix("#"));
            structureParsers.Add(new Integer());
            structureParsers.Add(new Double());
            structureParsers.Add(new SeparatedList(','));
            structureParsers.Add(new SeparatedList(' '));
        }

        public static Engine Instance { get; } = new Engine();

        private static readonly char[] multipleQuerySeparator = new[] { '|' };

        public static string GetResponse(string input, HttpRequest request = null)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Div("");
            }

            if (input.IndexOf('|') != -1)
            {
                var sb = new StringBuilder();
                sb.AppendLine("<div class=\"answersList\">");
                var multipleQueries = input.Split(multipleQuerySeparator, StringSplitOptions.RemoveEmptyEntries);
                foreach (var singleQuery in multipleQueries)
                {
                    var result = Instance.GetSingleResponseWorker(singleQuery, request);
                    if (result != null)
                    {
                        sb.AppendLine("<div class=\"answerBlock\">");
                        sb.AppendLine(DivClass(singleQuery, "answerBlockHeader"));
                        if (!string.IsNullOrEmpty(result))
                        {
                            sb.AppendLine(Div(result));
                        }
                        else
                        {
                            sb.AppendLine(DivClass("No result.", "note"));
                        }

                        sb.AppendLine("</div>");
                    }
                }

                if (sb.Length == 0)
                {
                    sb.AppendLine(DivClass("No results.", "note"));
                }

                sb.AppendLine("</div>");

                return sb.ToString();
            }
            else
            {
                var result = Instance.GetSingleResponseWorker(input, request);
                if (string.IsNullOrEmpty(result))
                {
                    result = DivClass("No results. Enter ? for help.", "note");
                }
                else
                {
                    result = DivClass(result, "answerBlock");
                }

                result = DivClass(result, "answersList");

                return result;
            }
        }

        public static object Parse(string input)
        {
            return Instance.ParseWorker(input);
        }

        public static T TryGetStructure<T>(object instance)
        {
            if (instance == null)
            {
                return default(T);
            }

            if (instance is T)
            {
                return (T)instance;
            }

            if (typeof(T) == typeof(byte[]))
            {
                var separatedList = TryGetStructure<SeparatedList>(instance);
                if (separatedList != null)
                {
                    var byteList = separatedList.GetStructuresOfType<Integer>();
                    if (byteList.Count == separatedList.Count && byteList.All(b => b.Value >= 0 && b.Value <= 255))
                    {
                        return (T)(object)(byteList.Select(b => (byte)b.Value).ToArray());
                    }
                }
            }

            IEnumerable<object> list = instance as IEnumerable<object>;
            if (list != null)
            {
                foreach (var item in list)
                {
                    var structure = TryGetStructure<T>(item);
                    if (structure != null)
                    {
                        return structure;
                    }
                }
            }

            var integer = instance as Integer;
            if (integer != null && integer.Kind == IntegerKind.Decimal && typeof(T) == typeof(Double))
            {
                // uhm... yeah.
                return (T)(object)new Double((double)((Integer)instance).Value);
            }

            return default(T);
        }

        private object ParseWorker(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            input = input.Trim();

            var list = new List<object>();

            foreach (var parser in structureParsers)
            {
                var result = parser.TryParse(input);
                if (result != null)
                {
                    list.Add(result);
                }
            }

            if (list.Count == 0)
            {
                return null;
            }
            else if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return list;
            }
        }

        public IEnumerable<IProcessor> Processors
        {
            get { return this.processors; }
        }

        private string GetSingleResponseWorker(string input, HttpRequest request = null)
        {
            var query = new Query(input);
            query.Request = request;

            var sb = new StringBuilder();
            foreach (var processor in processors)
            {
                var result = processor.GetResult(query);
                if (!string.IsNullOrEmpty(result))
                {
                    sb.AppendLine(result);
                }
            }

            var response = sb.ToString();
            if (!string.IsNullOrEmpty(response))
            {
                response = DivClass(response, "section");
            }

            return response;
        }
    }
}
