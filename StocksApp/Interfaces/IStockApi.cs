using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Interfaces
{
    public interface IStockApi
    {
        Task<List<Stock>> GetStocksAsync();
        Task<List<StockDetail>> GetStockDetailsAsync(string symbol);
    }

}
