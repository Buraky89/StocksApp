using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.External.Fmp
{
    public class FmpStock
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }
        public string ExchangeShortName { get; set; }
        public string Price { get; set; }
        public string Name { get; set; }
    }
}
