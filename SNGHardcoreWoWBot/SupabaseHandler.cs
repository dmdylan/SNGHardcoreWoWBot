﻿using DSharpPlus.Entities;
using SNGHardcoreWoWBot.Models;
using Supabase;
using System.Diagnostics;

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

            if (result != null)
                return result;
            else
                return null;
        }

        public static async Task RemovePlayer(DiscordUser user)
        {
            await supabase.From<Player>().Where(x => x.DiscordID == user.Id).Delete();
        }

        #endregion Player Tasks

        #region Character Tasks

        public static async Task AddNewCharacter(DiscordUser user, string characterName, string characterRace, string characterClass)
        {
            var model = new Character
            {
                CharacterName = characterName,
                CharacterRace = characterRace,
                CharacterClass = characterClass,
                CharacterAliveStatus = true,
                CharacterLevel = 1,
                CharacterOwner = user.Id
            };

            await supabase.From<Character>().Insert(model);
            Debug.WriteLine("Added new character");
        }

        public static async Task<Character> RetrieveSinglePlayerCharacter(DiscordUser user, string characterName)
        {
            var player = await RetrievePlayer(user);

            var character = await supabase.From<Character>().Where(x => x.CharacterOwner == user.Id
            && x.CharacterName.ToLower() == characterName.ToLower()).Single();

            return character;
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

            if (result != null && !result.CharacterAliveStatus)
                return;

            await supabase.From<Character>()
                .Where(x => x.CharacterName == character)
                .Set(x => x.CharacterLevel, result.CharacterLevel++)
                .Update();
        }

        public static async Task<bool> UpdateCharacterStatus(DiscordUser discordUser, string characterName)
        {
            var character = await RetrieveSinglePlayerCharacter(discordUser, characterName);

            if (character != null)
            {
                await supabase.From<Character>()
                    .Where(x => x.CharacterName == characterName)
                    .Set(x => x.CharacterAliveStatus, false)
                    .Update();

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Character Tasks
    }
}