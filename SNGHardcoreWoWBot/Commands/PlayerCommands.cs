using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SupabaseStuff;

namespace SNGHardcoreWoWBot.Commands
{
    public class PlayerCommands : BaseCommandModule
    {
        [Command("commands")]
        public async Task PlayerHelpCommand(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            var playerEmbed = new DiscordEmbedBuilder()
            {
                Title = "Player Command List",
                Color = new DiscordColor("#15f1f1")
            };

            playerEmbed.AddField("!register", "Add yourself to the database");
            playerEmbed.AddField("!unregister", "Remove yourself to the database");
            playerEmbed.AddField("!addcharacter CharacterName CharacterRace CharacterClass", "Adds a new character at level 1");
            playerEmbed.AddField("!addcharacter CharacterName CharacterRace CharacterClass CurrentLevel", "Adds a new character at level #");
            playerEmbed.AddField("!remove CharacterName CharacterID", "Remove a saved character");
            playerEmbed.AddField("!deathcount", "Retrieve the total number of deaths for yourself");
            playerEmbed.AddField("!deathcount @mention", "Retrieve the total number of deaths for @mention");

            var characterEmbed = new DiscordEmbedBuilder()
            {
                Title = "Character Command List",
                Color = new DiscordColor("#15f1f1")
            };

            characterEmbed.AddField("!ding CharacterName CharacterID", "Level up a character");
            characterEmbed.AddField("!death CharacterName CharacterID", "Change the status of your character");
            characterEmbed.AddField("!characters", "Retrieve a list of all your characters");
            characterEmbed.AddField("!characters @mention", "Retrieve a list of all @mention's characters");

            await commandContext.Channel.SendMessageAsync(embed: playerEmbed);
            await commandContext.Channel.SendMessageAsync(embed: characterEmbed);
        }

        #region Player Management Commands

        [Command("register")]
        public async Task RegisterPlayerCommand(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            if (await SupabaseHandler.RetrievePlayer(commandContext.User) != null)
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

        #endregion Player Management Commands

        #region Character Management Commands

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName, string race, string characterClass)
        {
            await commandContext.TriggerTypingAsync();

            bool[] infoTest = CheckProperCharacterInfo(characterName, race, characterClass);

            if (infoTest.Contains(false))
            {
                if (!infoTest[0])
                    await commandContext.RespondAsync("Invalid character name!");

                if (!infoTest[1])
                    await commandContext.RespondAsync("Incorrect character race!");

                if (!infoTest[2])
                    await commandContext.RespondAsync("Incorrect character class!");
            }
            else
            {
                await SupabaseHandler.AddNewCharacter(commandContext.User, characterName, race, characterClass);

                await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
            }
        }

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName, string race, string characterClass, int level)
        {
            await commandContext.TriggerTypingAsync();

            bool[] infoTest = CheckProperCharacterInfo(characterName, race, characterClass);

            if (infoTest.Contains(false))
            {
                if (!infoTest[0])
                    await commandContext.RespondAsync("Invalid character name!");

                if (!infoTest[1])
                    await commandContext.RespondAsync("Incorrect character race!");

                if (!infoTest[2])
                    await commandContext.RespondAsync("Incorrect character class!");
            }
            else
            {
                await SupabaseHandler.AddNewCharacter(commandContext.User, characterName, race, characterClass, level);

                await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
            }
        }

        [Command("remove")]
        public async Task RemovePlayerCharacter(CommandContext commandContext, string characterName, int characterID)
        {
            await commandContext.TriggerTypingAsync();

            var deleteTask = SupabaseHandler.RemoveCharacter(commandContext.User, characterName, characterID);

            if (deleteTask.IsCompletedSuccessfully)
            {
                await commandContext.RespondAsync($"{characterName} removed!");
            }
            else
            {
                if (deleteTask.Exception == null)
                    await commandContext.RespondAsync($"Could not remove {characterName} because {characterName} has already been removed or was never saved!");
                else
                    await commandContext.RespondAsync($"Could not remove {characterName} because {deleteTask.Exception}");
            }
        }

        #endregion Character Management Commands

        #region Player Stats Commands

        [Command("deathcount")]
        public async Task GetPlayerDeathCount(CommandContext commandContext)
        {
            await commandContext.TriggerTypingAsync();

            var player = await SupabaseHandler.RetrievePlayer(commandContext.User);

            if (player == null)
            {
                await commandContext.RespondAsync($"Cannot find player {commandContext.User.Username}!");
                return;
            }

            var deathCount = await SupabaseHandler.CheckPlayerDeathCount(commandContext.User);

            await commandContext.RespondAsync($"Total deaths for {commandContext.User.Username}: {deathCount}");
        }

        [Command("deathcount")]
        public async Task GetPlayerDeathCount(CommandContext commandContext, DiscordMember member)
        {
            await commandContext.TriggerTypingAsync();

            var player = await SupabaseHandler.RetrievePlayer(member);

            if (player == null)
            {
                await commandContext.RespondAsync($"Cannot find player {member.Mention}!");
                return;
            }

            var deathCount = await SupabaseHandler.CheckPlayerDeathCount(member);

            await commandContext.RespondAsync($"Total deaths for {member.Mention}: {deathCount}");
        }

        #endregion Player Stats Commands

        private bool[] CheckProperCharacterInfo(string characterName, string race, string characterClass)
        {
            bool characterNameIsCorrect = true;
            bool raceIsCorrect = false;
            bool characterIsCorrect = false;

            var name = characterName.ToCharArray();

            for (int i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                    characterNameIsCorrect = false;
            }

            for (int i = 0; i < Constants.RaceNamesArray.Length; i++)
            {
                if (Constants.RaceNamesArray[i] == race)
                {
                    raceIsCorrect = true;
                    break;
                }
            }

            for (int i = 0; i < Constants.ClassNamesArray.Length; i++)
            {
                if (Constants.ClassNamesArray[i] == characterClass)
                {
                    characterIsCorrect = true;
                    break;
                }
            }

            bool[] result = new[] { characterNameIsCorrect, raceIsCorrect, characterIsCorrect };
            return result;
        }
    }
}