using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Common.Services
{
    public class StockAggregator
    {
        public void Analyze()
        {

            // Find all of the stocks that we analyzed three days ago
            var stocks = File.ReadAllLines("D://UserApps//Stock.log").ToList().AsQueryable()
                         .Where(x => x.Substring(0, 20) == DateTime.UtcNow.AddDays(-3).ToString("yyyy-MM-dd"));

            // Regroup these stocks by there name, maybe in the log we can put json
            // Find the first { and then json convert the rest of the string
            // Once we have that then we can do a group by, count on the stock name
            //stocks = stocks.Select(x => JsonConvert.DeserializeObject<Stock>(x.Substring(x.IndexOf('{'), x.Length)));

            // For the top 20 stocks look up there previous price and their current price
            // Put this back into a log, and now the StockPageAnalyzer can load this
            // on startup and check to see stocks that commonly show up but dont change price
            // Then they can be blacklisted and make the app smarter, probably need a ttl on these
        }
    }
}
