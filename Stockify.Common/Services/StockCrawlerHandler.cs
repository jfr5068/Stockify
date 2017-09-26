using log4net;
using System;
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
        private static readonly ILog Log = LogManager.GetLogger(typeof(StockCrawlerHandler));
        private static bool IsRunning = false;

        public static void Handle(object source, ElapsedEventArgs e)
        {
            try
            {
                if(!IsRunning)
                {
                    IsRunning = true;
                    Log.Info("Timer fired, triggering new chatter scan");
                    var seedUrls = ConfigurationManager.AppSettings["Seeds"].Split(',').ToList();
                    StockPageAnalyzer analyzer = new StockPageAnalyzer();
                    WebCrawler crawler = new WebCrawler(seedUrls, analyzer);
                    crawler.Crawl();
                    analyzer.RankAll();
                    analyzer.PrintRanked();
                    Log.Info("Chatter scan finished!");
                }
                else
                {
                    Log.Warn("A chatter scan is already running, ignoring this one");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to run analyzer", ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}
