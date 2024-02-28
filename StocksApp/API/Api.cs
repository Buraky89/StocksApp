using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp
{
    public static class StocksApi
    {
        public static List<Model.Stock> GetMockStocks()
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

        public static List<StockDetail> GetMockStockDetails()
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


    }
}
