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

    }
}
