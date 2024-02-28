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
        public List<DateRangeOption> DateRangeOptions { get; private set; }
        public MockStockApi()
        {
            TimelineOptions = new List<TimelineOption>
            {
                new TimelineOption { Label = "5M", Value = "5min", IsDefault = true },
                new TimelineOption { Label = "1H", Value = "1hour", IsDefault = false },
                new TimelineOption { Label = "1Y", Value = "1year", IsDefault = false }
            };
            DateRangeOptions = new List<DateRangeOption>
            {
                new DateRangeOption { Label = "1 Day", Value = 1, IsDefault = true },
                new DateRangeOption { Label = "15 Days", Value = 15, IsDefault = false },
                new DateRangeOption { Label = "1 Month", Value = 30, IsDefault = false }
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

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol, string desiredTimelineOption = "5min", int desiredDateRangeOption = 15)
        {
            return new List<StockDetail>
            {
                new StockDetail
                {
                    Date = "2023-09-08 15:55:00",
                    Open = 178,
                    Low = 177.99m,
                    High = 178.34m,
                    Close = 178.19m,
                    Volume = 2640606
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:50:00",
                    Open = 177.93m,
                    Low = 177.79m,
                    High = 178.00m,
                    Close = 177.995m,
                    Volume = 1188267
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:45:00",
                    Open = 178.01m,
                    Low = 177.9m,
                    High = 178.14m,
                    Close = 177.925m,
                    Volume = 757194
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:40:00",
                    Open = 178.105m,
                    Low = 177.91m,
                    High = 178.12m,
                    Close = 178.00m,
                    Volume = 578531
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:35:00",
                    Open = 178.13m,
                    Low = 177.95m,
                    High = 178.178m,
                    Close = 178.11m,
                    Volume = 523998
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:30:00",
                    Open = 177.94m,
                    Low = 177.84m,
                    High = 178.175m,
                    Close = 178.13m,
                    Volume = 534214
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:25:00",
                    Open = 178.0409m,
                    Low = 177.855m,
                    High = 178.13m,
                    Close = 177.945m,
                    Volume = 445069
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:20:00",
                    Open = 178.0074m,
                    Low = 177.81m,
                    High = 178.2185m,
                    Close = 178.04m,
                    Volume = 618178
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:15:00",
                    Open = 178.27m,
                    Low = 177.89m,
                    High = 178.325m,
                    Close = 178.00m,
                    Volume = 744619
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:10:00",
                    Open = 178.35m,
                    Low = 178.21m,
                    High = 178.41m,
                    Close = 178.2721m,
                    Volume = 509946
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:05:00",
                    Open = 178.24m,
                    Low = 178.215m,
                    High = 178.39m,
                    Close = 178.34m,
                    Volume = 605998
                },
                new StockDetail
                {
                    Date = "2023-09-08 15:00:00",
                    Open = 178.2m,
                    Low = 177.86m,
                    High = 178.29m,
                    Close = 178.2499m,
                    Volume = 951134
                },
                new StockDetail
                {
                    Date = "2023-09-08 14:55:00",
                    Open = 178.0401m,
                    Low = 178.02m,
                    High = 178.2998m,
                    Close = 178.205m,
                    Volume = 616673
                },
                new StockDetail
                {
                    Date = "2023-09-08 14:50:00",
                    Open = 178.485m,
                    Low = 178.02m,
                    High = 178.51m,
                    Close = 178.0401m,
                    Volume = 841008
                },
                new StockDetail
                {
                    Date = "2023-09-08 14:45:00",
                    Open = 178.69m,
                    Low = 178.43m,
                    High = 178.74m,
                    Close = 178.485m,
                    Volume = 386939
                },
                new StockDetail
                {
                    Date = "2023-09-08 14:40:00",
                    Open = 178.385m,
                    Low = 178.33m,
                    High = 178.755m,
                    Close = 178.69m,
                    Volume = 710823
                },
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