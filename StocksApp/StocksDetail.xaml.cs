using StocksApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaktionslogik für StocksDetail.xaml
    /// </summary>
    // In StocksDetail.xaml.cs
    public partial class StocksDetail : Window
    {
        private Stock _stock;

        public StocksDetail(Stock stock)
        {
            InitializeComponent();
            _stock = stock;
            LoadStockDetails();
        }

        private void LoadStockDetails()
        {
            // Assuming you have TextBlocks or other controls to show these details
            this.Title = _stock.Name; // Example of setting the window title to the stock name
                                      // Set other controls' text here based on the _stock object



            StockName.Text = _stock.Name;
            StockSymbol.Text = $"Symbol: {_stock.Symbol}";
            StockExchange.Text = $"Exchange: {_stock.Exchange}";

            // Mock data for detailed stock metrics
            StockDetailsListView.ItemsSource = StocksApi.GetMockStockDetails();

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Your navigation code to go back to the previous view goes here.
            // For a simple scenario, if this is a Window, you might just close this window:
            this.Close();

            // If you're using a navigation service or a more complex navigation structure, you would call that service/method here instead.
        }
    }

}
