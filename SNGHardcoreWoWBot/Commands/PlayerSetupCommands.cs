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

            if(await SupabaseHandler.RetrievePlayer(commandContext.User) != null)
            {
                await commandContext.RespondAsync($"Player is already registered!");
                return;
            }

            await SupabaseHandler.AddNewPlayer(commandContext.User);

            await commandContext.RespondAsync($"Registered {commandContext.User.Username}");
        }

        [Command("unregister")]
        public async Task UnregisterPlayerCommand(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            if (await SupabaseHandler.RetrievePlayer(commandContext.User) != null)
            {
                await SupabaseHandler.RemovePlayer(commandContext.User);
                await commandContext.RespondAsync($"{commandContext.User.Username} removed!");

            }
            else
            {
                await commandContext.RespondAsync($"{commandContext.User.Username} is not in the database!");
            }
        }

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName)
        {
            await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
        }
    }
}