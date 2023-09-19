using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using SNGHardcoreWoWBot.Commands;
using System.Configuration;

namespace DiscordStuff
{
    internal class DiscordHandler
    {
        public static async Task InitializeDiscord()
        {
            var discord = new DiscordClient(new DiscordConfiguration
            {
                Token = ConfigurationManager.AppSettings.Get("discord_token"),
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                MinimumLogLevel = LogLevel.Debug
            });

            var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new[] { "!" }
            });

            commands.RegisterCommands<PlayerCommands>();
            commands.RegisterCommands<CharacterCommands>();

            await discord.ConnectAsync();
        }
    }
}