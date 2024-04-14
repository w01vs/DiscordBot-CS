using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Reflection;

namespace DiscordBot
{
    public class Bot : IBot
    {
        private ServiceProvider? services;
        private readonly IConfiguration configuration;
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;

        public Bot(IConfiguration configuration)
        {
            this.configuration = configuration;

            DiscordSocketConfig config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            client = new DiscordSocketClient(config);
            commands = new CommandService();
        }

        public async Task StartAsync(ServiceProvider services)
        {
            string token = configuration["token"] ?? throw new Exception("Missing Discord token");
            Console.WriteLine("Starting bot...");

            client.Log += Log;
            this.services = services;

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly(), services);
            client.MessageReceived += HandleCommandAsync;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
        }

        public async Task StopAsync()
        {
            Console.WriteLine("Stopping bot...");
            await client.LogoutAsync();
            await client.StopAsync();
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            Console.WriteLine("Here");
            SocketUserMessage? message = msg as SocketUserMessage;
            if (msg.Author.IsBot || message == null) return;

            Console.Write($"{DateTime.Now.ToShortTimeString()} - {message.Author}: {message.Content}");

            int pos = 0;
            bool hasPrefix = message.HasStringPrefix("!", ref pos);

            if (hasPrefix)
            {
                await commands.ExecuteAsync(new SocketCommandContext(client, message), pos, services);
            }
        }
    }
}