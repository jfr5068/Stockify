using log4net;
using Newtonsoft.Json;
using Stockify.Common.Model;
using Stockify.Common.Services;
using Stockify.Common.Utility;
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
        private static readonly ILog Log = LogManager.GetLogger(typeof(StockController));

        // GET: api/Stock
        public IEnumerable<Stock> Get()
        {
            try
            {
                return StockifyUtility.ReadAllLines("D:\\home\\stockify\\currentChatter.txt").Select(x => JsonConvert.DeserializeObject<Stock>(x));
            }
            catch (Exception ex)
            {
                Log.Error("Unable to read current chatter", ex);
                return null;
            }
        }
    }
}
