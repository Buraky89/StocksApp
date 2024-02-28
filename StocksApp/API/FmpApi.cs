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
        private const string HistoricalChartEndpoint = "https://financialmodelingprep.com/api/v3/historical-chart";
        private const string SearchEndpoint = "https://financialmodelingprep.com/api/v3/search";

        private readonly FmpMappers _fmpMappers;
        public List<TimelineOption> TimelineOptions { get; private set; }

        public FmpApi(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _fmpMappers = new FmpMappers();
            TimelineOptions = new List<TimelineOption>
            {
                new TimelineOption { Label = "5M", Value = "5min", IsDefault = true },
                new TimelineOption { Label = "1H", Value = "1hour", IsDefault = false },
                new TimelineOption { Label = "1Y", Value = "1year", IsDefault = false }
            };
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"{StocksEndpoint}?apikey={ApiKey}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allStocks = JsonConvert.DeserializeObject<List<FmpStock>>(content);
            return FmpStocksToStocks(allStocks);
        }

        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol, string desiredTimelineOption = "5M")
        {
            var timelineOption = TimelineOptions.Find(option => option.Value == desiredTimelineOption);
            if (timelineOption == null)
            {
                throw new ArgumentException("Invalid timeline option specified.", nameof(desiredTimelineOption));
            }

            var from = "2023-08-10";
            var to = "2023-09-10";
            var detailsEndpoint = $"{HistoricalChartEndpoint}/{timelineOption.Value}/{symbol}?from={from}&to={to}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(detailsEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var allDetails = JsonConvert.DeserializeObject<List<FmpStockDetail>>(content);

            return FmpStockDetailsToStockDetails(allDetails);
        }

        public async Task<List<Stock>> SearchAsync(string query)
        {
            var searchEndpoint = $"{SearchEndpoint}?query={query}&apikey={ApiKey}";

            var response = await _httpClient.GetAsync(searchEndpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var searchResults = JsonConvert.DeserializeObject<List<FmpSearchResult>>(content);

            return FmpSearchResultToStocks(searchResults.GetRange(0, Math.Min(10, searchResults.Count)));
        }

        private List<Stock> FmpSearchResultToStocks(List<FmpSearchResult> searchResults)
        {
            return _fmpMappers.FmpSearchResultToStocks(searchResults);
        }

        private List<Stock> FmpStocksToStocks(List<FmpStock> fmpStocks)
        {
            return _fmpMappers.FmpStocksToStocks(fmpStocks);
        }

        private List<StockDetail> FmpStockDetailsToStockDetails(List<FmpStockDetail> fmpStockDetails)
        {
            return _fmpMappers.FmpStockDetailsToStockDetails(fmpStockDetails);
        }
    }
}
