using DSharpPlus.Entities;
using SNGHardcoreWoWBot.Models;
using Supabase;

namespace SupabaseStuff
{
    internal class SupabaseHandler
    {
        private static readonly Client supabase = new("https://xsnpijpuuaxcfwkwxjxi.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhzbnBpanB1dWF4Y2Z3a3d4anhpIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY5NDQ3NzQ2MCwiZXhwIjoyMDEwMDUzNDYwfQ.XdePbVEOsSWH6OEVRvDeYJ6nStStyEeF3UVkuqSriws",
            new() { AutoConnectRealtime = true });

        public static async Task InitializeSupabase()
        {
            await supabase.InitializeAsync();
        }

        #region Player Tasks

        public static async Task AddNewPlayer(DiscordUser user)
        {
            var model = new Player
            {
                DiscordID = user.Id,
                DiscordName = user.Username
            };

            await supabase.From<Player>().Insert(model);
        }

        public static async Task<Player> RetrievePlayer(DiscordUser user)
        {
            var result = await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Single();

            return result;
        }

        public static async Task RemovePlayer(DiscordUser user)
        {
            await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Delete();
        }

        public static async Task<int> CheckPlayerDeathCount(DiscordUser user)
        {
            var player = await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Single();

            if (player == null)
                return 0;

            return player.DeathCount;
        }

        #endregion Player Tasks

        #region Character Tasks

        public static async Task AddNewCharacter(DiscordUser user, string characterName, string characterRace, string characterClass, int level = 1)
        {
            var model = new Character
            {
                CharacterName = characterName,
                CharacterRace = characterRace,
                CharacterClass = characterClass,
                CharacterAliveStatus = true,
                CharacterLevel = level,
                CharacterOwner = user.Id
            };

            await supabase.From<Character>().Insert(model);
        }

        public static async Task<Character> RetrieveSinglePlayerCharacter(DiscordUser user, string characterName)
        {
            var pc = await supabase.From<Character>()
                .Where(x => x.CharacterOwner == user.Id && x.CharacterName == characterName)
                .Single();

            return pc;
        }

        public static async Task<List<Character>> RetrieverAllPlayerCharacters(DiscordUser discordUser)
        {
            List<Character> list = new List<Character>();

            var player = await RetrievePlayer(discordUser);

            if (player != null)
            {
                var result = await supabase.From<Character>()
                    .Select("*")
                    .Where(x => x.CharacterOwner == discordUser.Id)
                    .Get();

                list = result.Models;
            }

            return list;
        }

        public static async Task LevelUpCharacter(DiscordUser user, string character)
        {
            var result = await RetrieveSinglePlayerCharacter(user, character);

            if (result != null && result.CharacterAliveStatus == false)
                return;

            if (result != null)
            {
                result.CharacterLevel += 1;

                await result.Update<Character>();
            }
        }

        public static async Task SetCharacterDeath(DiscordUser discordUser, string characterName)
        {
            var player = await RetrievePlayer(discordUser);
            var character = await RetrieveSinglePlayerCharacter(discordUser, characterName);

            if (character != null)
            {
                player.DeathCount += 1;
                character.CharacterAliveStatus = false;

                await player.Update<Player>();
                await character.Update<Character>();
            }
        }

        #endregion Character Tasks
    }
}