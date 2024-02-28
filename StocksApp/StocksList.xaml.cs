using StocksApp.API;
using StocksApp.Interfaces;
using StocksApp.Model;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace StocksApp
{
    /// <summary>
    /// Interaction logic for StocksList.xaml
    /// </summary>
    public partial class StocksList : Window
    {
        private readonly IStockApi _stockApi;

        public StocksList()
        {
            InitializeComponent();
            // TODO: setup dependency injection and remove this
            _stockApi = new FmpApi(new System.Net.Http.HttpClient());
            LoadMockData();
        }

        private async void LoadMockData()
        {
            StocksListView.ItemsSource = await _stockApi.GetStocksAsync();
        }

        private void StocksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StocksListView.SelectedItem is Stock selectedStock)
            {
                var detailWindow = new StocksDetail(selectedStock);
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
                    var searchResults = await _stockApi.SearchAsync(searchQuery);
                    StocksListView.ItemsSource = searchResults;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during search: {ex.Message}");
                    // Handle exceptions or errors as appropriate
                }
            }
        }

    }
}