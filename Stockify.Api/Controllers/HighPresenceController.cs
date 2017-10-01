using log4net;
using Newtonsoft.Json;
using Stockify.Common.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Stockify.Common.Services.StockAggregator;

namespace Stockify.Api.Controllers
{
    public class HighPresenceController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(StockController));

        public IEnumerable<StockPresence> Get()
        {
            try
            {
                return StockifyUtility.ReadAllLines("D:\\home\\stockify\\highPresence.txt").Select(x => JsonConvert.DeserializeObject<StockPresence>(x)).OrderByDescending(x => x.Occurrences).Take(1000);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to read high presence", ex);
                return null;
            }
        }
    }
}
