using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Common.Services
{
    public interface IPageAnalyzer
    {
        void Analyze(string url, string contents);
    }
}
