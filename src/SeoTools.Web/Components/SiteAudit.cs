using System.Xml;
using Microsoft.AspNetCore.Components;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using LogLevel = OpenQA.Selenium.LogLevel;

namespace SeoTools.Web.Components;

public class SiteAudit : ComponentBase
{

    [Inject]
    private IHttpClientFactory? ClientFactory { get; set; }

    public string? InputString { get; set; }
    public string? RobotsContent { get; set; }
    public IReadOnlyDictionary<string, string>? ParsedRobotsContent { get; set; }
    public IEnumerable<string>? Urls { get; set; }

    public string? SitemapContent { get; set; }

    public async Task TestUrlAsync()
    {
        var client = ClientFactory.CreateClient();
        var robots = await client.GetAsync(InputString + "robots.txt");

        if (!robots.IsSuccessStatusCode)
        {
            return;
        }

        var robotsContent = await robots.Content.ReadAsStringAsync();

        RobotsContent = robotsContent;
        ParsedRobotsContent = ParseRobotsContent(robotsContent);

        if (ParsedRobotsContent.TryGetValue("sitemap", out var sitemapUrl))
        {
            var sitemapResponse = await client.GetAsync(sitemapUrl);
            if (!sitemapResponse.IsSuccessStatusCode)
            {
                return;
            }

            var sitemapContent = await sitemapResponse.Content.ReadAsStringAsync();
            SitemapContent = sitemapContent;

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sitemapContent);
            var urls = xmlDocument.GetElementsByTagName("loc").OfType<XmlNode>().Select(x => x.InnerText).Where(x => !string.IsNullOrEmpty(x));
            Urls = urls;
            
            var driverService = ChromeDriverService.CreateDefaultService();
            var driver = new ChromeDriver(driverService);
            driver.Navigate().GoToUrl(Urls.First());

            var resources = driver.ExecuteScript("performance.getEntriesByType(\"resource\")");

            driver.Close();

        }
    }

    public IReadOnlyDictionary<string, string> ParseRobotsContent(string rawRobotsContent)
    {
        var lines = rawRobotsContent.Split('\n');

        return lines.ToDictionary(x => x.Split(':', 2)[0].ToLower(), x => x.Split(':', 2)[1]);

    }

}
