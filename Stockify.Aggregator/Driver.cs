using Stockify.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Aggregator
{
    class Driver
    {
        static void Main(string[] args)
        {
            StockAggregator agg = new StockAggregator();
            agg.Analyze();
        }
    }
}
