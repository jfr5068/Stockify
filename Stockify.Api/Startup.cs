﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Timers;
using Stockify.Common.Services;
using log4net;

[assembly: OwinStartup(typeof(Stockify.Api.Startup))]

namespace Stockify.Api
{
    public partial class Startup
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();
            ConfigureAuth(app);

            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(StockCrawlerHandler.Handle);
            aTimer.Interval = 30 * 60 * 1000;
            aTimer.Enabled = true;
            Log.Info("Started Up!");
        }
    }
}
