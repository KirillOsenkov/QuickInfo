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
                @"");
        }

        private void T(string input, string expectedHtml)
        {
            var engine = new Engine();
            var query = new Query(input);
            var actualResult = engine.GetResults(query).First().resultText;
            Assert.Equal(expectedHtml, actualResult);
        }
    }
}
