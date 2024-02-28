using StocksApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksApp.Interfaces
{
    public interface IStockApi
    {
        Task<List<Stock>> GetStocksAsync();
        Task<List<StockDetail>> GetStockDetailsAsync(string symbol);
        Task<List<Stock>> SearchAsync(string query);
    }
}
