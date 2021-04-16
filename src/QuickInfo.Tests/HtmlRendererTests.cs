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
    <tr>
      <td>Crimson</td>
      <td><a href=""?%23DC143C"" onclick=""searchFor(&quot;#DC143C&quot;);return false;"">#DC143C</a>
      </td>
      <td><a href=""?%23DC143C"" onclick=""searchFor(&quot;#DC143C&quot;);return false;""><div style=""background:#DC143C;width:60px;height:16px""></div>
        </a>
      </td>
    </tr>
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
            var answerNode = engine.GetResults(query).First().resultNode;
            var actualResult = HtmlRenderer.RenderObject(answerNode);
            Assert.Contains(expectedHtml, actualResult);
        }
    }
}
