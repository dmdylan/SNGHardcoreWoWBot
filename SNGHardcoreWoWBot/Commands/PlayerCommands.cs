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
            playerEmbed.AddField("!deathcount", "Retrieve the total number of deaths for yourself");
            playerEmbed.AddField("!deathcount @mention", "Retrieve the total number of deaths for @mention");

            var characterEmbed = new DiscordEmbedBuilder()
            {
                Title = "Character Command List",
                Color = new DiscordColor("#15f1f1")
            };

            characterEmbed.AddField("!ding CharacterName", "Level up a character");
            characterEmbed.AddField("!death CharacterName", "Change the status of your character");
            characterEmbed.AddField("!characters", "Retrieve a list of all your characters");
            characterEmbed.AddField("!characters @mention", "Retrieve a list of all @mention's characters");

            await commandContext.Channel.SendMessageAsync(embed: playerEmbed);
            await commandContext.Channel.SendMessageAsync(embed: characterEmbed);
        }

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

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName, string race, string characterClass)
        {
            await commandContext.TriggerTypingAsync();

            string infoTest = CheckProperCharacterInfo(race, characterClass);

            if (!string.IsNullOrEmpty(infoTest))
            {
                if (infoTest == "race")
                {
                    await commandContext.RespondAsync("Incorrect character race!");
                }
                else if (infoTest == "character")
                {
                    await commandContext.RespondAsync("Incorrect character class!");
                }
                else
                {
                    await commandContext.RespondAsync("Incorrect character race and class!");
                }
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

            string infoTest = CheckProperCharacterInfo(race, characterClass);

            if (!string.IsNullOrEmpty(infoTest))
            {
                if (infoTest == "race")
                {
                    await commandContext.RespondAsync("Incorrect character race!");
                }
                else if (infoTest == "character")
                {
                    await commandContext.RespondAsync("Incorrect character class!");
                }
                else
                {
                    await commandContext.RespondAsync("Incorrect character race and class!");
                }
            }
            else
            {
                await SupabaseHandler.AddNewCharacter(commandContext.User, characterName, race, characterClass, level);

                await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
            }
        }

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

        private string CheckProperCharacterInfo(string race, string characterClass)
        {
            bool raceIsCorrect = false;
            bool characterIsCorrect = false;

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

            if (raceIsCorrect && characterIsCorrect)
            {
                return string.Empty;
            }
            else if (!raceIsCorrect && characterIsCorrect)
            {
                return "race";
            }
            else if (raceIsCorrect && !characterIsCorrect)
            {
                return "character";
            }
            else
            {
                return "both";
            }
        }
    }
}