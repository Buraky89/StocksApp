using StocksApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StocksApp.Interfaces
{
    public interface IStockApi
    {
        Task<List<Stock>> GetStocksAsync();
        Task<List<StockDetail>> GetStockDetailsAsync(string symbol, string desiredTimelineOption = "5min", int howManyDays = 15);
        Task<List<Stock>> SearchAsync(string query);

        List<TimelineOption> TimelineOptions { get; }

        List<DateRangeOption> DateRangeOptions { get; }
    }
}
