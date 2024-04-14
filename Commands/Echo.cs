using Discord.Commands;

namespace DiscordBot.Commands
{
    public class Echo : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        [Summary("Echoes a message.")]
        public async Task EchoAsync([Remainder] string message)
        {
            await ReplyAsync(message);
        }
    }
}
