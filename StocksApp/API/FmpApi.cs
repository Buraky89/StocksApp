using Newtonsoft.Json;
using StocksApp.External.Fmp;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Collections.Generic;
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

        // Create an instance of FmpMappers
        private readonly FmpMappers _fmpMappers;

        public FmpApi(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _fmpMappers = new FmpMappers(); // Initialize the mapper
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"{StocksEndpoint}?apikey={ApiKey}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allStocks = JsonConvert.DeserializeObject<List<FmpStock>>(content);
            // Return only the top 5 items
            return FmpStocksToStocks(allStocks);
        }

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol)
        {
            var from = "2023-08-10";
            var to = "2023-09-10";
            var detailsEndpoint = $"{HistoricalChartEndpoint}/{symbol}?from={from}&to={to}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(detailsEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allDetails = JsonConvert.DeserializeObject<List<FmpStockDetail>>(content);

            // Return only the top 5 items
            return FmpStockDetailsToStockDetails(allDetails);
        }

        public async Task<List<Stock>> SearchAsync(string query)
        {
            var searchEndpoint = $"{SearchEndpoint}?query={query}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(searchEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var searchResults = JsonConvert.DeserializeObject<List<FmpSearchResult>>(content);

            // Return only the top 10 items
            return FmpSearchResultToStocks(searchResults.GetRange(0, Math.Min(10, searchResults.Count)));
        }

        // Mapper method for converting FmpSearchResult to Stock
        private List<Stock> FmpSearchResultToStocks(List<FmpSearchResult> searchResults)
        {
            // Use the FmpMappers instance to map search results to stocks
            return _fmpMappers.FmpSearchResultToStocks(searchResults);
        }

        // Mapper method for converting FmpStock to Stock
        private List<Stock> FmpStocksToStocks(List<FmpStock> fmpStocks)
        {
            return _fmpMappers.FmpStocksToStocks(fmpStocks);
        }

        // Mapper method for converting FmpStockDetail to StockDetail
        private List<StockDetail> FmpStockDetailsToStockDetails(List<FmpStockDetail> fmpStockDetails)
        {
            return _fmpMappers.FmpStockDetailsToStockDetails(fmpStockDetails);
        }
    }
}
