using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SNGHardcoreWoWBot.Models;
using SupabaseStuff;

namespace SNGHardcoreWoWBot.Commands
{
    internal class CharacterCommands : BaseCommandModule
    {
        #region Status Commands

        [Command("death")]
        public async Task ChangeCharacterStatus(CommandContext commandContext, string characterName)
        {
            await commandContext.TriggerTypingAsync();

            await SupabaseHandler.SetCharacterDeath(commandContext.User, characterName);

            await commandContext.RespondAsync($"RIP {characterName}");

            var character = await SupabaseHandler.RetrieveSinglePlayerCharacter(commandContext.User, characterName);

            var embed = CreateCharacterDiscordEmbed(character);

            await commandContext.Channel.SendMessageAsync(embed: embed);
        }

        [Command("ding")]
        public async Task LevelUpCharacter(CommandContext commandContext, string character)
        {
            await commandContext.TriggerTypingAsync();

            var result = await SupabaseHandler.RetrieveSinglePlayerCharacter(commandContext.User, character);

            if (result != null)
            {
                await SupabaseHandler.LevelUpCharacter(commandContext.User, character);

                await commandContext.RespondAsync($"{character} has reached level {result.CharacterLevel + 1}! Grats!");
            }
            else
            {
                await commandContext.RespondAsync($"Could not find character!");
            }
        }

        #endregion Status Commands

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
                await commandContext.RespondAsync($"Could not find {member.Mention}! Make sure they are registered first!");
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
                await commandContext.RespondAsync($"Could not find any characters for {member.Mention}!");
        }

        #endregion Character List Commands

        #region Helper Methods

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

        #endregion Helper Methods
    }
}