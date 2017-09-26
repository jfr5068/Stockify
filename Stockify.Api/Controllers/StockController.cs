using log4net;
using Stockify.Common.Model;
using Stockify.Common.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Stockify.Api.Controllers
{
    public class StockController : ApiController
    {
        private DateTime LastRefreshed;
        private List<Stock> cached;
        private static readonly ILog Log = LogManager.GetLogger(typeof(StockController));

        // GET: api/Stock
        public IEnumerable<Stock> Get()
        {
            try
            {
                if (LastRefreshed == null || DateTime.UtcNow.Subtract(LastRefreshed).TotalMinutes > 30)
                {
                    var log = ConfigurationManager.AppSettings["logFile"];
                    StockPageAnalyzer analyzer = new StockPageAnalyzer();
                    analyzer.Analyze("Log", File.ReadAllText(log));
                    analyzer.RankAll();
                    cached = analyzer.GetLogRanked();
                    LastRefreshed = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to analyze and return log data", ex);
                throw;
            }

            return cached;
        }

        // GET: api/Stock/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Stock
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Stock/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Stock/5
        public void Delete(int id)
        {
        }
    }
}
