using System;
using System.Linq;
using Xunit;

namespace QuickInfo.Tests
{
    public class HtmlRendererTests
    {
        [Fact]
        public void TestColorTable()
        {
            T("color",
@"<table style=""border-spacing: 10px"">
  <tr>
    <td>
      <a href=""?Black"" onclick=""searchFor(&quot;Black&quot;);return false;"">
        <div class=""swatch"" style=""background:Black"">
        </div>
      </a>
      <div class=""swatchName"">
        Black</div>
    </td>
    <td>");
        }

        [Fact]
        public void TestColor()
        {
            T("red",
@"");
        }

        private void T(string input, string expectedHtml)
        {
            var engine = new Engine();
            var query = new Query(input);
            var answerNode = engine.GetResults(query).First().resultText;
            var actualResult = HtmlRenderer.RenderObject(answerNode);
            Assert.Contains(expectedHtml, actualResult);
        }
    }
}
