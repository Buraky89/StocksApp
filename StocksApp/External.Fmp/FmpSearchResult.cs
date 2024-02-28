using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.External.Fmp
{
    public class FmpSearchResult
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string StockExchange { get; set; }
        public string ExchangeShortName { get; set; }
    }
}
