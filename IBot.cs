using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    interface IBot
    {
        Task StartAsync(ServiceProvider services);
        Task StopAsync();
    }
}
