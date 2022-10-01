using Newtonsoft.Json.Linq;
using WebReaper.Core.DOM;
using WebReaper.Core.Scraper;
using WebReaper.Domain.Parsing;

namespace WebReaper.IntegrationTests
{
    public class ScraperTests
    {
        [Fact]
        public async Task ListTest()
        {
            List<JObject> result = new List<JObject>();

            var scraper = new Scraper()
                .WithStartUrl("https://kniga.io/books/pelevin-viktor-snuff0")
                .Parse(new Schema {
                    new ElementList("genres", ".badge.bg-success.rounded-pill"),
                })
                .AddScrapedDataHandler(x => result.Add(x));

            _ = scraper.Run(1);

            await Task.Delay(10000);

            await scraper.Stop();

            await Task.Delay(1000);

            Assert.NotEmpty(result);
        }
    }
}