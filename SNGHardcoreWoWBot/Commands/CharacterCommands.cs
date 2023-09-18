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

            bool characterUpdated = await SupabaseHandler.UpdateCharacterStatus(commandContext.User, characterName);

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

        [Command("characters")]
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
            {
                for (int i = 0; i < list.Count; i++)
                {
                    DiscordEmbed embed = CreateCharacterDiscordEmbed(list[i]);

                    await commandContext.Channel.SendMessageAsync(embed: embed);
                }
            }
            else
                await commandContext.RespondAsync($"Could not find any characters for {commandContext.User.Username}!");
        }

        [Command("characters")]
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
            {
                foreach (var character in list)
                {
                    var embed = CreateCharacterDiscordEmbed(character);
                    await commandContext.Channel.SendMessageAsync(embed: embed);
                }
            }
            else
                await commandContext.RespondAsync($"Could not find any characters for {member.Username}!");
        }

        [Command("ding")]
        public async Task LevelUpCharacter(CommandContext commandContext, string character)
        {
            await commandContext.TriggerTypingAsync();

            var result = await SupabaseHandler.RetrieveSinglePlayerCharacter(commandContext.User, character);

            if (result != null)
            {
                await SupabaseHandler.LevelUpCharacter(commandContext.User, character);
                await commandContext.RespondAsync($"{character} has reached level {result.CharacterLevel}! Grats!");
            }
            else
            {
                await commandContext.RespondAsync($"Could not find character!");
            }
        }

        #endregion Character List Commands

        private DiscordEmbed CreateCharacterDiscordEmbed(Character character)
        {
            var builder = new DiscordEmbedBuilder()
            {
                Title = character.CharacterName,
                Color = CharacterColorPicker(character.CharacterClass),
            };

            builder.AddField($"{character.CharacterRace} {character.CharacterClass}", "\u200B");
            builder.AddField($"Level: {character.CharacterLevel}", "\u200B");
            builder.AddField($"{(character.CharacterAliveStatus ? "Alive" : "Dead")}", "\u200B");

            return builder;
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
                    return DiscordColor.None;
            }
        }
    }
}