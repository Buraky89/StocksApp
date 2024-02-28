﻿using StocksApp.API;
using StocksApp.Interfaces;
using StocksApp.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Defaults;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;

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
            LoadStocksDateRangeButtons();
        }

        private async void LoadStockDetails()
        {
            try
            {
                LoadingText.Text = "Loading...";
                LoadingText.Visibility = Visibility.Visible;
                StockDetailsListView.Visibility = Visibility.Collapsed;

                this.Title = _stock.Name;

                StockName.Text = _stock.Name;
                StockSymbol.Text = $"Symbol: {_stock.Symbol}";
                StockExchange.Text = $"Exchange: {_stock.Exchange}";

                var stockDetails = await _stockApi.GetStockDetailsAsync(_stock.Symbol);

                // Setup the chart
                var series = new LineSeries
                {
                    Title = "High",
                    Values = new ChartValues<DateTimePoint>(),
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                };

                foreach (var detail in stockDetails)
                {
                    string dateString = detail.Date;
                    string format = "yyyy-MM-dd HH:mm:ss";
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime parsedDate = DateTime.ParseExact(dateString, format, provider);

                    series.Values.Add(new DateTimePoint(parsedDate, Convert.ToDouble(detail.High)));
                }

                Chart.Series = new SeriesCollection { series };
                Chart.AxisX[0].LabelFormatter = value => new DateTime((long)value).ToString("d");



                LoadingText.Visibility = Visibility.Collapsed; // Hide loading text after successful load
                StockDetailsListView.ItemsSource = stockDetails;
                StockDetailsListView.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                LoadingText.Text = "Error";
                MessageBox.Show($"Error loading stock details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private void LoadStocksDateRangeButtons()
        {
            foreach (var dateRangeOption in _stockApi.DateRangeOptions)
            {
                var button = new Button
                {
                    Content = dateRangeOption.Label,
                    Tag = dateRangeOption.Value
                };
                button.Click += DateRangeOptionButton_Click;
                StocksDateRangePanel.Children.Add(button);
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
                    var stockDetails = await _stockApi.GetStockDetailsAsync(_stock.Symbol, timelineOptionValue, 15);

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

        private async void DateRangeOptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int dateRangeOptionValue)
            {
                try
                {
                    LoadingText.Visibility = Visibility.Visible; // Show loading text
                    StockDetailsListView.Visibility = Visibility.Collapsed; // Hide stock details

                    // Load stock details for the selected timeline option
                    var stockDetails = await _stockApi.GetStockDetailsAsync(_stock.Symbol, "5min", dateRangeOptionValue);

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
