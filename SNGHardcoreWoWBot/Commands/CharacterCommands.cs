using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SupabaseStuff;

namespace SNGHardcoreWoWBot.Commands
{
    internal class CharacterCommands : BaseCommandModule
    {
        [Command("characterstatus")]
        public async Task ChangeCharacterStatus(CommandContext commandContext, string characterName, string status)
        {
            await commandContext.RespondAsync($"{characterName} {status}");
        }

        #region Character List Commands

        [Command("characterlist")]
        public async Task GetAllCharacters(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            if (await SupabaseHandler.RetrievePlayer(commandContext.User) == null)
            {
                await commandContext.RespondAsync($"Could not find {commandContext.User.Username}! Make sure they are registered first!");
                return;
            }

            var list = await SupabaseHandler.RetrieverAllPlayerCharacters(commandContext.User);

            if (list != null)
                await commandContext.RespondAsync(list.Count.ToString());
            else
                await commandContext.RespondAsync($"Could not find any characters for {commandContext.User.Username}!");
        }

        [Command("characterlist")]
        public async Task GetAllCharacters(CommandContext commandContext, DiscordMember member)
        {
            await commandContext.TriggerTypingAsync();

            if (await SupabaseHandler.RetrievePlayer(member) == null)
            {
                await commandContext.RespondAsync($"Could not find {member.Username}! Make sure they are registered first!");
                return;
            }

            var list = await SupabaseHandler.RetrieverAllPlayerCharacters(member);

            if (list != null)
                await commandContext.RespondAsync(list.Count.ToString());
            else
                await commandContext.RespondAsync($"Could not find any characters for {member.Username}!");
        }

        #endregion Character List Commands
    }
}