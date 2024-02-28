using System.Collections.Generic;
using StocksApp.Model;
using StocksApp.External.Fmp;

namespace StocksApp.External.Fmp
{
    public class FmpMappers
    {
        public List<Stock> FmpSearchResultToStocks(List<FmpSearchResult> searchResults)
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

        public List<Stock> FmpStocksToStocks(List<FmpStock> fmpStocks)
        {
            var stocks = new List<Stock>();
            foreach (var fmpStock in fmpStocks)
            {
                stocks.Add(new Stock
                {
                    Symbol = fmpStock.Symbol,
                    Exchange = fmpStock.Exchange,
                    ExchangeShortName = fmpStock.ExchangeShortName,
                    Price = "", // You can assign default value or leave it blank
                    Name = fmpStock.Name
                });
            }
            return stocks;
        }

        public List<StockDetail> FmpStockDetailsToStockDetails(List<FmpStockDetail> fmpStockDetails)
        {
            var stockDetails = new List<StockDetail>();
            foreach (var fmpStockDetail in fmpStockDetails)
            {
                stockDetails.Add(new StockDetail
                {
                    Date = fmpStockDetail.Date,
                    Open = fmpStockDetail.Open,
                    Low = fmpStockDetail.Low,
                    High = fmpStockDetail.High,
                    Close = fmpStockDetail.Close,
                    Volume = fmpStockDetail.Volume
                });
            }
            return stockDetails;
        }
    }
}
