using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SNGHardcoreWoWBot.Models;
using SupabaseStuff;

namespace SNGHardcoreWoWBot.Commands
{
    internal class CharacterCommands : BaseCommandModule
    {
        [Command("characterstatus")]
        public async Task ChangeCharacterStatus(CommandContext commandContext, string characterName, string status)
        {
            await commandContext.TriggerTypingAsync();

            bool characterUpdated = await SupabaseHandler.UpdateCharacterStatus(commandContext.User, characterName, status);

            if (characterUpdated)
            {
                await commandContext.RespondAsync($"{characterName}'s status updated!");
            }
            else
            {
                await commandContext.RespondAsync($"Could not update character status!");
            }
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

        private DiscordEmbed CreateCharacterDiscordEmbed(Character character)
        {
            var buidler = new DiscordEmbedBuilder
            {
                Title = character.CharacterName,
                Color = CharacterColorPicker(character.CharacterClass),
            };

            buidler.AddField($"{character.CharacterRace} {character.CharacterClass}", null);
            buidler.AddField($"{character.CharacterLevel}", null);
            buidler.AddField($"{(character.CharacterAliveStatus ? "Alive" : "Dead")}", null);

            return buidler;
        }

        private DiscordColor CharacterColorPicker(string characterClass)
        {
            switch (characterClass.ToLower())
            {
                case "druid":
                    return Constants.DruidClassColorHexCode;

                case "hunter":
                    return Constants.HunterClassColorHexCode;

                case "mage":
                    return Constants.MageClassColorHexCode;

                case "paladin":
                    return Constants.PaladinClassColorHexCode;

                case "priest":
                    return Constants.PriestClassColorHexCode;

                case "rogue":
                    return Constants.RogueClassColorHexCode;

                case "shaman":
                    return Constants.ShamanClassColorHexCode;

                case "warlock":
                    return Constants.WarlockClassColorHexCode;

                case "warrior":
                    return Constants.WarriorClassColorHexCode;

                default:
                    return null;
            }
        }
    }
}