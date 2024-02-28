using Newtonsoft.Json;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Net.Http;
using System.Threading.Tasks;

namespace StocksApp.API
{
    public class FmpApi : IStockApi
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "b89odf7V3VVFAcjAs8VOL5PnPXaGnDXq";
        private const string StocksEndpoint = "https://financialmodelingprep.com/api/v3/stock/list";
        private const string HistoricalChartEndpoint = "https://financialmodelingprep.com/api/v3/historical-chart/5min";
        private const string SearchEndpoint = "https://financialmodelingprep.com/api/v3/search";

        public FmpApi(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"{StocksEndpoint}?apikey={ApiKey}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allStocks = JsonConvert.DeserializeObject<List<Stock>>(content);
            // Return only the top 5 items
            return allStocks.GetRange(0, Math.Min(5, allStocks.Count));
        }

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol)
        {
            var from = "2023-08-10";
            var to = "2023-09-10";
            var detailsEndpoint = $"{HistoricalChartEndpoint}/{symbol}?from={from}&to={to}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(detailsEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allDetails = JsonConvert.DeserializeObject<List<StockDetail>>(content);

            // Return only the top 5 items
            return allDetails.GetRange(0, Math.Min(5, allDetails.Count));
        }


        public async Task<List<Stock>> SearchAsync(string query)
        {
            var searchEndpoint = $"{SearchEndpoint}?query={query}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(searchEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var searchResults = JsonConvert.DeserializeObject<List<FmpSearchResult>>(content);

            // Return only the top 10 items
            return MapSearchResultsToStocks(searchResults.GetRange(0, Math.Min(10, searchResults.Count)));
        }



        private List<Stock> MapSearchResultsToStocks(List<FmpSearchResult> searchResults)
        {
            var stocks = new List<Stock>();
            foreach (var result in searchResults)
            {
                stocks.Add(new Stock
                {
                    Symbol = result.Symbol,
                    Exchange = result.StockExchange,
                    ExchangeShortName = result.ExchangeShortName,
                    Price = "", // You can assign default value or leave it blank
                    Name = result.Name
                });
            }
            return stocks;
        }

        internal class FmpSearchResult
        {
            public string Symbol { get; set; }
            public string Name { get; set; }
            public string Currency { get; set; }
            public string StockExchange { get; set; }
            public string ExchangeShortName { get; set; }
        }

    }
}
