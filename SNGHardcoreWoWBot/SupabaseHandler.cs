using DSharpPlus.Entities;
using SNGHardcoreWoWBot.Models;
using Supabase;
using System.Reflection.Metadata.Ecma335;

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
            var result = await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Get();

            if (result != null)
                return result.Model;
            else
                return null;
        }

        public static async Task RemovePlayer(DiscordUser user)
        {
            await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Delete();
        }

        public static async Task AddNewCharacter(DiscordUser user, string characterName, string characterRace, string characterClass)
        {
            var model = new Character
            {
                CharacterName = characterName,
                CharacterRace = characterRace,
                CharacterClass = characterClass,
                CharacterAliveStatus = true
            };

            await supabase.From<Character>().Insert(model);
        }

        public static async Task<Character> RetrieveSinglePlayerCharacter(DiscordUser user, string characterName)
        {
            var player = RetrievePlayer(user);

            var character = await supabase.From<Character>().Where(x => x.CharacterOwner == user.Id 
            && x.CharacterName.ToLower() == characterName.ToLower()).Get();

            return character.Model;
        }

        public static async Task<List<Character>> RetrieverAllPlayerCharacters(DiscordUser discordUser)
        {
            List<Character> list = new List<Character>();

            var player = RetrievePlayer(discordUser);

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
    }
}