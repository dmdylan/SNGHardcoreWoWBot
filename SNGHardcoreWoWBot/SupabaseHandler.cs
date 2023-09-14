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

        public static async Task AddNewPlayer(DiscordUser user)
        {
            var model = new Players
            {
                DiscordID = user.Id,
                DiscordName = user.Username
            };

            await supabase.From<Players>().Insert(model);
        }

        public static async Task AddNewCharacter(DiscordUser user, string characterName, string characterRace, string characterClass)
        {
            var model = new Characters
            {
                CharacterName = characterName,
                CharacterRace = characterRace,
                CharacterClass = characterClass,
                CharacterAliveStatus = true
            };

            await supabase.From<Characters>().Insert(model);
        }
    }
}