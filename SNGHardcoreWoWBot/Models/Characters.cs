using Postgrest.Attributes;
using Postgrest.Models;

namespace SNGHardcoreWoWBot.Models
{
    [Table("characters")]
    internal class Characters : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("character_owner")]
        public ulong CharacterOwner { get; set; }

        [Column("character_name")]
        public string CharacterName { get; set; } = string.Empty;

        [Column("race")]
        public string CharacterRace { get; set; } = string.Empty;

        [Column("class")]
        public string CharacterClass { get; set; } = string.Empty;

        [Column("status")]
        public bool CharacterAliveStatus { get; set; }
    }
}