using Microsoft.Extensions.DependencyInjection;
using OpcClient.Services;
using System.Windows;
using OpcClient.ViewModels;

namespace OpcClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            var baseUrl = Environment.GetEnvironmentVariable("OPC_API_BASE") ?? "http://localhost:8080";

            services.AddHttpClient("api", c => c.BaseAddress = new Uri(baseUrl));
            services.AddSingleton<OpcApiClient>();

            // ViesModels
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<TagsViewModel>();
            services.AddSingleton<TrendViewModel>();
            services.AddSingleton<SummaryViewModel>();

            Services = services.BuildServiceProvider();

            var mainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainViewModel>()
            };
            mainWindow.Show();
        }
    }
}
