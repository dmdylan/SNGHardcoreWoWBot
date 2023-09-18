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