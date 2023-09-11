using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using SNGHardcoreWoWBot.Commands;

namespace SNGHardcoreWoWBot
{
    internal class Program
    {
        static async Task Main(string[] args)
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
                StringPrefixes = new[] {"!"}
            });

            commands.RegisterCommands<BaseCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}