﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Stockify.Common.Services
{
    public class StockCrawlerHandler
    {
        public static void Handle(object source, ElapsedEventArgs e)
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
