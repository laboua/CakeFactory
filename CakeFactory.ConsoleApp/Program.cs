using CakeFactory.ConsoleApp.Configuration;
using CakeFactory.Service;
using CakeFactory.Service.Helpers;
using CakeFactory.Service.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CakeFactory.ConsoleApp
{
    public static class Program
    {
        private static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var cakeFactorySettingsConfig = new CakeSettingsConfig();
            configuration.Bind(cakeFactorySettingsConfig);

            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ICakeService, CakeService>(serviceProvider =>
                {
                    return new CakeService(new DurationSettings(
                        cakeFactorySettingsConfig.DurationSettings.PrepareDurationInterval.GetRandom().ToMilliseconds(),
                        cakeFactorySettingsConfig.DurationSettings.CookDuration.ToMilliseconds(),
                        cakeFactorySettingsConfig.DurationSettings.PackageDuration.ToMilliseconds(),
                        cakeFactorySettingsConfig.ParallelismSettings.PrepareMaxDegree.ToMilliseconds(),
                        cakeFactorySettingsConfig.ParallelismSettings.CookMaxDegree.ToMilliseconds(),
                        cakeFactorySettingsConfig.ParallelismSettings.PrepareMaxDegree.ToMilliseconds(),
                        cakeFactorySettingsConfig.DurationSettings.ReportingDuration.ToMilliseconds()
                        ));
                })
                .BuildServiceProvider();

            var cakService = serviceProvider.GetService<ICakeService>();

            int initialStock = cakeFactorySettingsConfig.Stock;

            var stock = new Stock(initialStock);

            InitialScreenConfig(initialStock);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            await cakService.RunAsync(stock);
            stopWatch.Stop();

            Console.WriteLine($"Stock initial: {initialStock} - Stock final: {stock.Size}");
            Console.WriteLine($"Temps de traitement: {stopWatch.Elapsed}");
            Console.WriteLine("\nPress any key to exit ..");
            Console.ReadKey();
        }

        private static void InitialScreenConfig(int stockNb)
        {
            Console.Title = "Usine de fabrication de gâteau";
            string initialStockTitle = $" * ******************** Stock initial: { stockNb } *********************";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - initialStockTitle.Length) / 2, Console.CursorTop);
            Console.WriteLine(initialStockTitle);
            Console.ResetColor();
        }
    }
}
