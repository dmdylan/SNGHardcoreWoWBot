using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using SNGHardcoreWoWBot.Commands;
using Supabase;

namespace SNGHardcoreWoWBot
{
    internal class Program
    {
        //TODO: Move everything to own class?
        private static async Task Main(string[] args)
        {
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "MTE0OTkyODIwNDI5MTgwNTI0Ng.GarktY.Teht5l081cCloh26Fc4EATsB4Z8H2TOpPf8IMM",
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                MinimumLogLevel = LogLevel.Debug
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<BaseCommands>();

            string url = "https://xsnpijpuuaxcfwkwxjxi.supabase.co";
            string key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhzbnBpanB1dWF4Y2Z3a3d4anhpIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTQ0Nzc0NjAsImV4cCI6MjAxMDA1MzQ2MH0.UdatJdbEs2r6kdHuo5cTkNWDckCxs8uVFjip7_EB99E";

            SupabaseOptions options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            Client supabase = new Client(url, key, options);

            await discord.ConnectAsync();
            await supabase.InitializeAsync();

            await Task.Delay(-1);
        }
    }
}