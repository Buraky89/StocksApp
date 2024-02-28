using Newtonsoft.Json;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.API
{
    public class FmpApi : IStockApi
    {
        private readonly HttpClient _httpClient;

        public FmpApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            // Implement the logic to fetch stock data from the real API
            // Example URL: "https://api.example.com/stocks"
            var response = await _httpClient.GetAsync("your_api_endpoint_for_stocks");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Stock>>(content);
        }

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol)
        {
            // Implement the logic to fetch stock details from the real API
            // Example URL: "https://api.example.com/stocks/details/{symbol}"
            var response = await _httpClient.GetAsync($"your_api_endpoint_for_stock_details/{symbol}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<StockDetail>>(content);
        }
    }

}
