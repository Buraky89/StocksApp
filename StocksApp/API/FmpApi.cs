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

        public List<DateRangeOption> DateRangeOptions { get; private set; }

        public FmpApi(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _fmpMappers = new FmpMappers();
            TimelineOptions = new List<TimelineOption>
            {
                new TimelineOption { Label = "1m", Value = "1min", IsDefault = true },
                new TimelineOption { Label = "5m", Value = "5min", IsDefault = false },
                new TimelineOption { Label = "15m", Value = "15min", IsDefault = false },
                new TimelineOption { Label = "30m", Value = "30min", IsDefault = false },
                new TimelineOption { Label = "1H", Value = "1hour", IsDefault = false },
                new TimelineOption { Label = "4H", Value = "4hour", IsDefault = false },
            };
            DateRangeOptions = new List<DateRangeOption>
            {
                new DateRangeOption { Label = "1 Day", Value = 1, IsDefault = true },
                new DateRangeOption { Label = "15 Days", Value = 15, IsDefault = false },
                new DateRangeOption { Label = "30 Days", Value = 30, IsDefault = false },
                new DateRangeOption { Label = "90 Days", Value = 90, IsDefault = false },
                new DateRangeOption { Label = "180 Months", Value = 180, IsDefault = false },
                new DateRangeOption { Label = "365 Days", Value = 365, IsDefault = false }
            };
        }

        public async Task<List<Stock>> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"{StocksEndpoint}?apikey={ApiKey}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content) || content == "[]")
                {
                    throw new ApiException(1, "empty data returned");
                }
                var allStocks = JsonConvert.DeserializeObject<List<FmpStock>>(content);
                return FmpStocksToStocks(allStocks);
            }
            else
            {
                await HandleErrorResponse(response);
                return null;
            }
        }

        private async Task HandleErrorResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new ApiException(2, "internal server error");
            }
            else
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    if (errorObj.ContainsKey("Error Message"))
                    {
                        throw new ApiException(0, errorObj["Error Message"]);
                    }
                    else
                    {
                        throw new ApiException(-1, "An unknown error occurred.");
                    }
                }
                catch (JsonException)
                {
                    throw new ApiException(-1, "Failed to parse error response.");
                }
            }
        }



        public async Task<List<StockDetail>> GetStockDetailsAsync(string symbol, string desiredTimelineOption = "5M", int desiredDateRangeOption = 15)
        {


            try
            {
                var timelineOption = TimelineOptions.Find(option => option.Value == desiredTimelineOption);
                if (timelineOption == null)
                {
                    throw new ArgumentException("Invalid timeline option specified.", nameof(desiredTimelineOption));
                }

                var dateRangeOption = DateRangeOptions.Find(option => Convert.ToInt32(option.Value) == desiredDateRangeOption);
                if (dateRangeOption == null)
                {
                    throw new ArgumentException("Invalid dateRange option specified.", nameof(desiredDateRangeOption));
                }

                int daysAgo = Convert.ToInt32(dateRangeOption.Value);
                DateTime fromDate = DateTime.Now.AddDays(-daysAgo);
                DateTime toDate = DateTime.Now;

                var from = fromDate.ToString("yyyy-MM-dd");
                var to = toDate.ToString("yyyy-MM-dd");

                var detailsEndpoint = $"{HistoricalChartEndpoint}/{timelineOption.Value}/{symbol}?from={from}&to={to}&apikey={ApiKey}";

                var response = await _httpClient.GetAsync(detailsEndpoint);
                if (!response.IsSuccessStatusCode)
                {
                    await HandleErrorResponse(response);
                    return null; // This line is for compilation; HandleErrorResponse will throw.
                }

                var content = await response.Content.ReadAsStringAsync();
                var allDetails = JsonConvert.DeserializeObject<List<FmpStockDetail>>(content);

                return FmpStockDetailsToStockDetails(allDetails);
            }
            catch (ApiException ex)
            {
                throw ex;
            }

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
