using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SupabaseStuff;

namespace SNGHardcoreWoWBot.Commands
{
    public class PlayerSetupCommands : BaseCommandModule
    {
        [Command("register")]
        public async Task RegisterPlayerCommand(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            await SupabaseHandler.AddNewPlayer(commandContext.User);

            await commandContext.RespondAsync($"Registered {commandContext.User.Username}");
        }

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName)
        {
            await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
        }
    }
}