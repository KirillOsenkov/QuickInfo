using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace QuickInfo.Tests
{
    public class IntegrationTests
    {
        [Theory]
        [InlineData("a",         @"<div class=""mainAnswerText"">LATIN SMALL LETTER A</div>")]
        [InlineData("%C3%A9",    @"<div class=""mainAnswerText"">LATIN SMALL LETTER E WITH ACUTE</div>")]
        [InlineData("%E2%80%AF", @"<div class=""mainAnswerText"">NARROW NO-BREAK SPACE</div>")]
        [InlineData("U%2BA7FB",  @"<div class=""mainAnswerText"">LATIN EPIGRAPHIC LETTER REVERSED F</div>")]
        [InlineData("F0 9F 8D 92 F0 9F 8D 87", @"🍒🍇")]
        [InlineData("100 EUR", "100")]
        public async Task TestQuery(string query, string output)
        {
            var response = await GetResponse(query);
            Assert.Contains(output, response);
        }

        [Theory]
        [InlineData("100 EUR in USD", @"100")]
        public async Task TestCurrency(string query, string output)
        {
            var response = await GetResponse(query);
            Assert.Contains(output, response);
        }

        public async Task<string> GetResponse(string query)
        {
            await using var application = new WebApplicationFactory<Program>();
            using var client = application.CreateClient();

            var response = await client.GetStringAsync($"api/Answers?query={query}");
            return response;
        }
    }
}
