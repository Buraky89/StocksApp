using StocksApp.API;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StocksApp
{
    public partial class StocksDetail : Window
    {
        private Stock _stock;
        private readonly IStockApi _stockApi;

        public StocksDetail(Stock stock)
        {
            InitializeComponent();
            _stock = stock;
            // TODO: setup dependency injection and remove this
            _stockApi = new FmpApi(new System.Net.Http.HttpClient());
            LoadStockDetails();
            LoadTimelineOptionButtons();
        }

        private async void LoadStockDetails()
        {
            // Show loading text
            LoadingText.Visibility = Visibility.Visible;
            StockDetailsListView.Visibility = Visibility.Collapsed;

            // Assuming you have TextBlocks or other controls to show these details
            this.Title = _stock.Name; // Example of setting the window title to the stock name
                                      // Set other controls' text here based on the _stock object

            StockName.Text = _stock.Name;
            StockSymbol.Text = $"Symbol: {_stock.Symbol}";
            StockExchange.Text = $"Exchange: {_stock.Exchange}";

            // Mock data for detailed stock metrics
            StockDetailsListView.ItemsSource = await _stockApi.GetStockDetailsAsync(_stock.Symbol);

            // Hide loading text and show ListView after data is loaded
            LoadingText.Visibility = Visibility.Collapsed;
            StockDetailsListView.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Your navigation code to go back to the previous view goes here.
            // For a simple scenario, if this is a Window, you might just close this window:
            this.Close();

            // If you're using a navigation service or a more complex navigation structure, you would call that service/method here instead.
        }
        private void LoadTimelineOptionButtons()
        {
            foreach (var timelineOption in _stockApi.TimelineOptions)
            {
                var button = new Button
                {
                    Content = timelineOption.Label,
                    Tag = timelineOption.Value // Store the timeline option value as the button's tag
                };
                button.Click += TimelineOptionButton_Click; // Wire up the click event
                TimelineOptionPanel.Children.Add(button); // Add the button to the panel
            }
        }

        private async void TimelineOptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string timelineOptionValue)
            {
                try
                {
                    LoadingText.Visibility = Visibility.Visible; // Show loading text
                    StockDetailsListView.Visibility = Visibility.Collapsed; // Hide stock details

                    // Load stock details for the selected timeline option
                    var stockDetails = await _stockApi.GetStockDetailsAsync(_stock.Symbol, timelineOptionValue);

                    // Update UI with loaded stock details
                    StockDetailsListView.ItemsSource = stockDetails;
                    StockDetailsListView.Visibility = Visibility.Visible; // Show stock details
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading stock details: {ex.Message}");
                    // Handle error loading stock details
                }
                finally
                {
                    LoadingText.Visibility = Visibility.Collapsed; // Hide loading text
                }
            }
        }
    }
}
