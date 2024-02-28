using StocksApp.API;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using StocksApp.Interfaces;

namespace StocksApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private readonly IServiceProvider _serviceProvider;

        //public App()
        //{
        //    var serviceCollection = new ServiceCollection();
        //    ConfigureServices(serviceCollection);
        //    _serviceProvider = serviceCollection.BuildServiceProvider();
        //}

        //private void ConfigureServices(IServiceCollection services)
        //{
        //    //services.AddHttpClient<IStockApi, MockStockApi>(); // Use this line for real API
        //                                                       // services.AddSingleton<IStockService, MockStockService>(); // Use this line for mock API

        //    services.AddSingleton<IStockApi, MockStockApi>(); // Use this line for mock API
        //    services.AddSingleton<StocksList>();


        //    // Register other services and view models as needed
        //}

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    var mainWindow = _serviceProvider.GetService<StocksList>(); // Ensure MainWindow is registered and its dependencies can be injected
        //    mainWindow?.Show();
        //}
    }

}
