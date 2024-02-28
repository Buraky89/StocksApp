﻿using StocksApp.API;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StocksApp
{
    public partial class StocksList : Window
    {
        private readonly IStockApi _stockApi;

        public StocksList(IStockApi stockApi)
        {
            InitializeComponent();
            _stockApi = stockApi;
            LoadMockData();
        }

        private async void LoadMockData()
        {
            // Show loading text
            LoadingText.Visibility = Visibility.Visible;
            StocksListView.Visibility = Visibility.Collapsed;
            var stocks = await _stockApi.GetStocksAsync();
            StocksListView.ItemsSource = stocks;
            // Hide loading text and show ListView
            LoadingText.Visibility = Visibility.Collapsed;
            StocksListView.Visibility = Visibility.Visible;
        }

        private void StocksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StocksListView.SelectedItem is Stock selectedStock)
            {
                var detailWindow = new StocksDetail(selectedStock, _stockApi);
                detailWindow.Show();
                // Reset the selection in the ListView
                StocksListView.SelectedItem = null;
            }
        }

        private async void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    var searchQuery = SearchBox.Text;
                    // Show loading text
                    LoadingText.Visibility = Visibility.Visible;
                    StocksListView.Visibility = Visibility.Collapsed;
                    var searchResults = await _stockApi.SearchAsync(searchQuery);
                    StocksListView.ItemsSource = searchResults;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during search: {ex.Message}");
                    // Handle exceptions or errors as appropriate
                }
                finally
                {
                    // Hide loading text and show ListView after search is complete
                    LoadingText.Visibility = Visibility.Collapsed;
                    StocksListView.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
