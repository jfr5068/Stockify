using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stockify.Common.Utility
{
    public class StockifyUtility
    {
        public static List<string> ReadAllLines(String path)
        {
            List<string> result = new List<string>();
            using (var csv = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(csv))
            {
                StringBuilder strB = new StringBuilder();
                while (!sr.EndOfStream)
                {
                    result.Add(sr.ReadLine());
                }
                return result;
            }
        }
    }
}
