using DiscordStuff;
using SupabaseStuff;

namespace SNGHardcoreWoWBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await DiscordHandler.InitializeDiscord();

            await SupabaseHandler.InitializeSupabase();

            await Task.Delay(-1);
        }
    }
}