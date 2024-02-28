using StocksApp.External.Fmp;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.API
{
    public class MockStockApi : IStockApi
    {
        public List<TimelineOption> TimelineOptions { get; private set; }
        public MockStockApi()
        {
            TimelineOptions = new List<TimelineOption>
            {
                new TimelineOption { Label = "5M", Value = "5min", IsDefault = true },
                new TimelineOption { Label = "1H", Value = "1hour", IsDefault = false },
                new TimelineOption { Label = "1Y", Value = "1year", IsDefault = false }
            };
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var stocks = new List<Model.Stock>
            {
                new Model.Stock
                {
                    Symbol = "PWP",
                    Exchange = "NASDAQ Global Select",
                    ExchangeShortName = "NASDAQ",
                    Price = "8.13",
                    Name = "Perella Weinberg Partners"
                }
            };
            return stocks;
        }

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol, string desiredTimelineOption = "5min")
        {
            return new List<StockDetail>
        {
                new StockDetail
                {
                    Date = "2023-03-02 16:00:00",
                    Open = 145.92m,
                    Low = 145.72m,
                    High = 146.00m,
                    Close = 145.79m,
                    Volume = 1492644
                },
                new StockDetail
                {
                    Date = "2023-03-02 15:00:00",
                    Open = 142.92m,
                    Low = 141.72m,
                    High = 143.00m,
                    Close = 141.79m,
                    Volume = 1492633
                }
            };
        }


        public async Task<List<Stock>> SearchAsync(string query)
        {
            return new List<Stock>
            {
                new Stock
                {
                    Symbol = "AA",
                    Name = "Alcoa Corporation",
                    Exchange = "New York Stock Exchange",
                    ExchangeShortName = "NYSE",
                    Price = "", // You can assign default value or leave it blank
                },
                new Stock
                {
                    Symbol = "AAU",
                    Name = "Almaden Minerals Ltd.",
                    Exchange = "American Stock Exchange",
                    ExchangeShortName = "AMEX",
                    Price = "", // You can assign default value or leave it blank
                }
            };
        }

    }
}