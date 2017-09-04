using Stockify.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> test = new List<string> { "https://finance.yahoo.com/quote/CAT?p=CAT", "http://www.marketwatch.com/investing/stock/cat", "http://investorplace.com/2017/08/dont-buy-microsoft-corporation-msft-stock-yet/#.Wa3I1LKGPIU" };
            StockPageAnalyzer analyzer = new StockPageAnalyzer();
            WebCrawler crawler = new WebCrawler(test, analyzer);
            crawler.Crawl();
            analyzer.PrintTop10();
        }
    }
}
