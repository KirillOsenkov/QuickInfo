using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static QuickInfo.HtmlFactory;

namespace QuickInfo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController : ControllerBase
    {
        [HttpGet]
        public string Get([FromQuery] string query)
        {
            string result = null;
            try
            {
                result = GetResponse(Instance, query, Request);
            }
            catch (Exception ex)
            {
                var text = DivClass(Escape(ex.ToString()), "exceptionStack");
                text = text + Div("<br/>Please open a new issue at " + A("https://github.com/KirillOsenkov/QuickInfo/issues/new") + " and paste the exception text above. Thanks and sorry for the inconvenience!");
                result = DivClass(text, "exception");
            }

            Response.Headers.Add("Cache-Control", new[] { "no-cache" });
            Response.Headers.Add("Pragma", new[] { "no-cache" });
            Response.Headers.Add("Expires", new[] { "-1" });
            Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Content-Type" });

            return result;
        }

        private static readonly char[] multipleQuerySeparator = new[] { '|' };

        public static Engine Instance { get; } = new Engine(
            typeof(Engine).Assembly,
            typeof(Ip).Assembly);

        private string GetResponse(Engine engine, string input, HttpRequest request = null)
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
                    var result = GetSingleResponseWorker(engine, singleQuery, request);
                    if (result == null)
                    {
                        result = DivClass("No results.", "note");
                    }

                    sb.AppendLine("<div class=\"answerBlock\">");
                    sb.AppendLine(DivClass(singleQuery, "answerBlockHeader"));

                    result = DivClass(result, "singleAnswerSection");

                    sb.AppendLine(result);

                    sb.AppendLine("</div>");
                }

                if (multipleQueries.Length == 0)
                {
                    sb.AppendLine(DivClass("No results.", "note"));
                }

                sb.AppendLine("</div>");

                return sb.ToString();
            }
            else
            {
                var result = GetSingleResponseWorker(engine, input, request);
                if (result == null)
                {
                    result = DivClass($"No results. {SearchLink("Enter ? for help.", "?")}", "note");
                }

                result = DivClass(Environment.NewLine + result, "answersList");

                return result;
            }
        }

        private string GetSingleResponseWorker(Engine engine, string input, HttpRequest request = null)
        {
            var query = new WebQuery(input);
            query.Request = request;

            var results = engine.GetResults(query);

            var response = HtmlRenderer.RenderObject(results);

            if (query.IsHelp)
            {
                response = DivClass(response, "singleAnswerSection");
            }

            return response;
        }
    }
}
