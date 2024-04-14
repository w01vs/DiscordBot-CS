using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

using DiscordBot.Commands;
using System.Globalization;

namespace DiscordBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var config = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly()).Build();
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IBot, Bot>()
                .AddSingleton<Echo>()
                .BuildServiceProvider();
            IBot bot = new Bot(config);
            await bot.StartAsync(services);
            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if(key.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("\n");
                    await bot.StopAsync();
                    return;
                }
            }
        }
    }
}