using StocksApp.API;
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
        private string _desiredTimelineOption = "5min";
        private int _howManyDays = 15;


        public StocksDetail(Stock stock, IStockApi stockApi)
        {
            InitializeComponent();
            _stock = stock;
            _stockApi = stockApi;

            LoadAndDisplayStockDetails(_desiredTimelineOption, _howManyDays).ConfigureAwait(false);

            LoadTimelineOptionButtons();
            LoadStocksDateRangeButtons();
            SetStockDetails();
        }
        private void SetStockDetails()
        {
            this.Title = _stock.Name;
            StockName.Text = _stock.Name;
            StockSymbol.Text = $"Symbol: {_stock.Symbol}";
            StockExchange.Text = $"Exchange: {_stock.Exchange}";
        }

        private SeriesCollection GenerateSeriesFromStockDetails(IEnumerable<StockDetail> stockDetails)
        {
            var series = new LineSeries
            {
                Title = "High",
                Values = new ChartValues<DateTimePoint>(),
                PointGeometry = DefaultGeometries.None,
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

            return new SeriesCollection { series };
        }

        private void UpdateChart(SeriesCollection series)
        {
            Chart.Series = series;
            Chart.AxisX[0].LabelFormatter = value => new DateTime((long)value).ToString("d");
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

        private async Task LoadAndDisplayStockDetails(string timelineOption, int days)
        {
            try
            {
                LoadingText.Visibility = Visibility.Visible;
                StockDetailsListView.Visibility = Visibility.Collapsed;

                var stockDetails = await _stockApi.GetStockDetailsAsync(_stock.Symbol, timelineOption, days);
                var series = GenerateSeriesFromStockDetails(stockDetails);
                UpdateChart(series);

                StockDetailsListView.ItemsSource = stockDetails;
                StockDetailsListView.Visibility = Visibility.Visible;
                LoadingText.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                StockDetailsListView.Visibility = Visibility.Hidden;
                LoadingText.Visibility = Visibility.Visible;
                LoadingText.Text = ex.Message.Length > 100 ? ex.Message.Substring(0, 100) : ex.Message;
                Chart.Visibility = Visibility.Collapsed;
                StocksDateRangePanel.Visibility = Visibility.Hidden;
                TimelineOptionPanel.Visibility = Visibility.Hidden;

                MessageBox.Show($"Error loading stock details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void TimelineOptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string timelineOptionValue)
            {
                _desiredTimelineOption = timelineOptionValue;
                await LoadAndDisplayStockDetails(_desiredTimelineOption, _howManyDays);
            }
        }

        private async void DateRangeOptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int dateRangeOptionValue)
            {
                _howManyDays = dateRangeOptionValue;
                await LoadAndDisplayStockDetails(_desiredTimelineOption, _howManyDays);
            }
        }

    }
}
