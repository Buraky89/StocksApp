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
        public StocksList()
        {
            InitializeComponent();
            LoadMockData();
        }

        private void LoadMockData()
        {
            StocksListView.ItemsSource = StocksApi.GetMockStocks();
        }

    }
}