using log4net;
using Newtonsoft.Json;
using Stockify.Common.Model;
using Stockify.Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Common.Services
{
    public class StockAggregator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StockAggregator));

        public void Analyze()
        {
            try
            {
                var files = Directory.GetFiles("D:\\home\\logfiles\\Stockify").ToList().AsQueryable().OrderByDescending(x => x).Take(20);
                var allStocks = new List<Stock>();

                foreach (var file in files)
                {
                    try
                    {
                        var stocks = StockifyUtility.ReadAllLines(file).ToList().AsQueryable()
                                 .Where(x => x.Contains("Stock:"))
                                 .Select(x => x.Substring(x.IndexOf('{'), x.Length - x.IndexOf('{')))
                                 .Select(x => JsonConvert.DeserializeObject<Stock>(x))
                                 .GroupBy(x => x.Ticker).Select(x => new Stock { Ticker = x.Key, Name = x.Max(y => y.Name), Rank = x.Sum(y => y.Rank) });
                        allStocks.AddRange(stocks.ToList());
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Unable to analyze file {file}", ex);
                    }
                }

                var stockPresence = allStocks.GroupBy(x => x.Ticker).Select(x => new StockPresence { Ticker = x.Key, Occurrences = x.Count() }).Where(x => x.Occurrences > files.Count() / 2);
                File.WriteAllLines("D:\\home\\stockify\\highPresence.txt", stockPresence.Select(x => JsonConvert.SerializeObject(x)));
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to analyze files", ex);
            }
        }

        public void Aggregate()
        {
            try
            {
                var highPresence = StockifyUtility.ReadAllLines("D:\\home\\stockify\\highPresence.txt").AsQueryable().Select(x => JsonConvert.DeserializeObject<StockPresence>(x)).ToList();

                var stocks = StockifyUtility.ReadAllLines(ConfigurationManager.AppSettings["logFile"]).ToList().AsQueryable()
                     .Where(x => x.Contains("Stock:"))
                     .Select(x => x.Substring(x.IndexOf('{'), x.Length - x.IndexOf('{')))
                     .Select(x => JsonConvert.DeserializeObject<Stock>(x))
                     .Where(x => !highPresence.Select(y => y.Ticker).Contains(x.Ticker))
                     .GroupBy(x => x.Ticker).Select(x => new Stock { Ticker = x.Key, Name = x.Max(y => y.Name), Rank = x.Sum(y => y.Rank) });

                File.WriteAllLines("D:\\home\\stockify\\currentChatter.txt", stocks.Select(x => JsonConvert.SerializeObject(x)));
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to find current chatter", ex);
            }
        }

        public class StockPresence
        {
            public string Ticker { get; set; }
            public int Occurrences { get; set; }
        }
    }
}
