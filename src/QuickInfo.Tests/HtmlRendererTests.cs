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
        <div class=""swatch"" style=""background:Black""></div>
      </a>
      <div class=""swatchName"">Black</div>
    </td>
    <td>");
        }

        [Fact]
        public void TestColor()
        {
            T("red",
@"<div class=""fixed"" style=""font-size:larger"">RGB(255,0,0) = #F00</div>
  <div style=""background:#F00;max-width:300px;height:50px""></div>
  <div class=""swatchName"">Red</div>
  <div class=""sectionHeader"">Closest named colors:</div>
  <table>
    <tr>");
        }

        [Fact]
        public void TestTes()
        {
            T("tes", "");
        }

        private void T(string input, string expectedHtml)
        {
            var engine = new Engine();
            var query = new Query(input);
            var answerNode = engine.GetResults(query);
            var actualResult = HtmlRenderer.RenderObject(answerNode);
            string expectedText = expectedHtml.TrimWhitespaceFromEachLine();
            string actualText = actualResult.TrimWhitespaceFromEachLine();
            Assert.Contains(expectedText, actualText);
        }
    }
}
