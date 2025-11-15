using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using McDonalds.ViewModels;
using McDonalds.Models.Core;
using McDonalds.Mappings;
using McDonalds.Wpf.Services;

namespace McDonalds.Wpf
{
    public partial class App : Application
    {
        public static IHost? Host { get; private set; }
        public static IServiceProvider? Services => Host?.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;

                    // HttpClient для API
                    services.AddHttpClient<ApiService>(client =>
                    {
                        client.BaseAddress = new Uri(config["ApiUrl"]);
                    });

                    services.AddAutoMapper(typeof(ApiMappingProfile));

                    // ViewModels
                    services.AddSingleton<ResourceManager>();
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<ResourceViewModel>();
                })
                .Build();

            await Host.StartAsync();

            var mainWindow = new MainWindow();
            mainWindow.DataContext = Services!.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (Host != null)
                await Host.StopAsync();
            base.OnExit(e);
        }
    }
}