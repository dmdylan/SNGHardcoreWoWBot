using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace SNGHardcoreWoWBot.Commands
{
    public class BaseCommands : BaseCommandModule
    {
        [Command("register")]
        public static async Task RegisterPlayerCommand(CommandContext commandContext)
        {
            await commandContext.RespondAsync($"Registered {commandContext.User.Username}");
        }

        [Command("addcharacter")]
        public static async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName)
        {
            await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
        }

        [Command("characterstatus")]
        public static async Task ChangeCharacterStatus(CommandContext commandContext, string characterName, string status)
        {
            await commandContext.RespondAsync($"{characterName} {status}");
        }
    }
}