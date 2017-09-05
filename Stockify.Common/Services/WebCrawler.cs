using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Common.Services
{
    public class WebCrawler
    {
        private List<string> rootSites;
        private IPageAnalyzer analyzer;

        public WebCrawler(List<string> rootSites, IPageAnalyzer analyzer)
        {
            this.rootSites = rootSites;
            this.analyzer = analyzer;
        }

        public void Crawl()
        {
            // For each root site pass the contents to the analyzer, we want to create a singleton here that
            // can track all pages.
            // Then the crawler will also scan the site for links to other pages, and pass them to the analyzer too
            // Limit the number of scans beneath a rootsite to be 10?
            foreach (string site in rootSites)
            {
                Crawl(site, true);
            }
        }

        public void Crawl(string site, bool findChildren)
        {
            try
            {
                Console.WriteLine($"Crawling: {site}");
                WebClient client = new WebClient();
                var content = client.DownloadString(site);
                this.analyzer.Analyze(site, content);

                if (findChildren)
                    FindAndCrawlLinks(site);
            }
            catch (Exception)
            {
                // Noop
            }
        }

        private void FindAndCrawlLinks(string site)
        {
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(site);
            int count = 0;
            // TODO: Should search these in random order so that we dont hit the same links over and over
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                if(count >= 20)
                {
                    return;
                }
                count++;
                var child = link.Attributes["href"].Value;
                if (child.ToLower().Contains("http"))
                {
                    Crawl(child, false);
                }
            }
        }
    }
}
