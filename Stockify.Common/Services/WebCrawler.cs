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
            foreach(string site in rootSites)
            {
                WebClient client = new WebClient();
                var content = client.DownloadString(site);
                this.analyzer.Analyze(site, content);
            }
        }
    }
}
