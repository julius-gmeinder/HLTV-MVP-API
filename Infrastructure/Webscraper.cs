using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;

namespace HLTV_API.Infrastructure
{
    public class Webscraper
    {
        private readonly IBrowser _browser;

        public Webscraper()
        {
            _browser = InitializeBrowser().Result;
        }

        private async Task<IBrowser> InitializeBrowser()
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            var extra = new PuppeteerExtra();

            extra.Use(new StealthPlugin()).Use(new AnonymizeUaPlugin());

            return await extra.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox", "--disable-setuid-sandbox" }
            });
        }

        public async Task<string> GetUrlAsync(string url, string selector)
        {
            var page = await _browser.NewPageAsync();

            const string customUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/119.0.0.0 Safari/537.36 OPR/105.0.0.0";
            await page.SetUserAgentAsync(customUserAgent);

            await page.GoToAsync(url);
            await page.WaitForSelectorAsync(selector);

            var html = await page.EvaluateFunctionAsync<string>($"() => document.querySelector('{selector}').outerHTML");
            
            await page.CloseAsync();

            return html;
        }
    }
}
