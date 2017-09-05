using Stockify.Common.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify
{
    class Program
    {
        static void Main(string[] args)
        {
            var seedUrls = ConfigurationManager.AppSettings["Seeds"].Split(',').ToList();
            StockPageAnalyzer analyzer = new StockPageAnalyzer();
            WebCrawler crawler = new WebCrawler(seedUrls, analyzer);
            crawler.Crawl();
            analyzer.RankAll();
            analyzer.PrintRanked();
        }
    }
}
