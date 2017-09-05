using Stockify.Common.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Stockify.Common.Services
{
    public class StockPageAnalyzer : IPageAnalyzer
    {
        private List<Stock> Stocks = new List<Stock>();
        private Dictionary<string, int> RankedStockChatter = new Dictionary<string, int>();
        private List<string> CommonWords = new List<string>();
        private Dictionary<string, Dictionary<string, int>> PageRanks = new Dictionary<string, Dictionary<string, int>>();
        private const int NUM_RANKS = 10;

        public StockPageAnalyzer()
        {
            GetCurrentStockInfo();
            GetCommonWords();
        }

        // Query for all possible stock tickers
        // For every page that is called in the analyze method
        // count the number of times any of these words show up
        // if a match happens then save it in a map with the site url
        // as the key
        public void Analyze(string url, string contents)
        {
            var contentsLower = contents.ToLower();
            foreach (Stock stock in Stocks)
            {
                int count = 0;
                // Count the number of times the stock ticker was found
                count += Regex.Matches(contentsLower, $" {stock.Ticker} ").Count;

                // Split the name of the company down to individual words
                // If the word is not a common word then get the number of matches
                // also multiply the count by the length of the word
                var nameWords = stock.Name.Split(' ');
                foreach(var word in nameWords)
                {
                    if(!CommonWords.Contains(word))
                    {
                        try
                        {
                            count += (Regex.Matches(contentsLower, $" {word} ").Count * word.Length);
                        }
                        catch (Exception)
                        {
                            // Noop, these mean special characters
                        }
                    }
                }

                if (stock.Name.Length > 8 && contentsLower.Contains(stock.Name))
                    count += 10000;

                UpdatePageRank(url, stock, count);
            }
        }

        /// <summary>
        /// This will go through each top 10 stocks from each page and then compare the rank from other pages
        /// it then takes the compared rank and adds it to the page rank and places this in the Ranked stock chatter
        /// </summary>
        public void RankAll()
        {
            foreach(var pageRank in PageRanks)
            {
                foreach(var comparingPageRank in PageRanks)
                {
                    if(pageRank.Key != comparingPageRank.Key)
                    {
                        var topRanked = pageRank.Value.OrderByDescending(x => x.Value).Take(NUM_RANKS);
                        foreach(var stock in topRanked)
                        {
                            int count = stock.Value + comparingPageRank.Value[stock.Key];
                            UpdateRankChatter(stock.Key, count, RankedStockChatter);
                        }
                    }
                }
            }
        }

        private void UpdatePageRank(string url, Stock stock, int count)
        {
            Dictionary<string, int> stockRanks;

            if (!PageRanks.TryGetValue(url, out stockRanks))
            {
                stockRanks = new Dictionary<string, int>();
                PageRanks.Add(url, stockRanks);
            }

            UpdateStockPageRank(stock, count, stockRanks);
        }

        private void UpdateStockPageRank(Stock stock, int count, Dictionary<string, int> stockRanks)
        {
            int fCount = 0;
            string key = $"{stock.Ticker}|{stock.Name}";
            if (stockRanks.TryGetValue(key, out fCount))
            {
                stockRanks[key] = count + fCount;
            }
            else
            {
                stockRanks.Add(key, count);
            }
        }

        private void UpdateRankChatter(string stock, int count, Dictionary<string, int> stockRanks)
        {
            int fCount = 0;
            if (stockRanks.TryGetValue(stock, out fCount))
            {
                stockRanks[stock] = count + fCount;
            }
            else
            {
                stockRanks.Add(stock, count);
            }
        }

        public void PrintRanked()
        {
            var ordered = RankedStockChatter.OrderByDescending(x => x.Value).Take(NUM_RANKS);
            foreach (var stock in ordered)
            {
                Console.WriteLine($"Stock: {stock.Key} Rank: {stock.Value}");
            }
        }

        private void GetCurrentStockInfo()
        {
            this.AddToStocks(File.ReadAllLines("C:\\Users\\ricejf\\Documents\\Personal\\Stockify\\Stocks\\nasdaq.csv").ToList());
            this.AddToStocks(File.ReadAllLines("C:\\Users\\ricejf\\Documents\\Personal\\Stockify\\Stocks\\nyse.csv").ToList());
        }

        private void AddToStocks(List<string> lines)
        {
            int count = 0;
            foreach (string line in lines)
            {
                if(count == 0)
                {
                    count++;
                    continue;
                }
                Stocks.Add(new Stock
                {
                    Ticker = line.Split(',')[0].ToLower().Replace("\"", ""),
                    Name = line.Split(',')[1].ToLower().Replace("\"", "")
                });
            }
        }

        private void GetCommonWords()
        {
            this.CommonWords = new List<string>();
            foreach(var line in File.ReadAllLines("C:\\Users\\ricejf\\Documents\\Personal\\Stockify\\Stocks\\commonWords.txt"))
            {
                this.CommonWords.Add(line.ToLower());
            }
        }
    }
}
