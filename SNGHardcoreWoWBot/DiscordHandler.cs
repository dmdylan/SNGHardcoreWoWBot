using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using SNGHardcoreWoWBot.Commands;

namespace DiscordStuff
{
    internal class DiscordHandler
    {
        public static async Task InitializeDiscord()
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

            commands.RegisterCommands<PlayerSetupCommands>();
            commands.RegisterCommands<CharacterCommands>();

            await discord.ConnectAsync();
        }
    }
}