using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Timers;
using Stockify.Common.Services;

[assembly: OwinStartup(typeof(Stockify.Api.Startup))]

namespace Stockify.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(StockCrawlerHandler.Handle);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
        }
    }
}
