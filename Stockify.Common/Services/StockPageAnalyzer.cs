﻿using Stockify.Common.Model;
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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                if (stock.Name.Length > 8 && contentsLower.Contains(stock.Name))
                    count += 10000;

                UpdatePageRank(url, stock, count);
            }
        }

        public void RankAll()
        {
            foreach(var pageRank in PageRanks)
            {
                // TODO: Need to get the top 10 ranks from each page, then check the ranks across other pages
                // Use some type of scale factor like multiply by the order of the rank in each corresponding page

                // Create a map of the urls and then put these maps beneath, when the process finished then
                // go through each map, compare the top 10 of each map with each of the other maps, multiply
                // the count of the map with the rank of the comparing map and then put it into a new map which will
                // then be sorted again at the end
            }
        }

        private void UpdatePageRank(string url, Stock stock, int count)
        {
            Dictionary<string, int> stockRanks = new Dictionary<string, int>();

            if (!PageRanks.TryGetValue(url, out stockRanks))
            {
                PageRanks.Add(url, stockRanks);
            }

            UpdateStockPageRank(stock, count, stockRanks);
        }

        private void UpdateStockPageRank(Stock stock, int count, Dictionary<string, int> stockRanks)
        {
            int fCount = 0;
            if(stockRanks.TryGetValue($"{stock.Ticker}|{stock.Name}", out fCount))
            {
                stockRanks[$"{stock.Ticker}|{stock.Name}"] = count + fCount;
            }
            else
            {
                stockRanks.Add($"{stock.Ticker}|{stock.Name}", count);
            }
        }

        public void PrintRanked()
        {
            var ordered = RankedStockChatter.OrderByDescending(x => x.Value).Take(10);
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
