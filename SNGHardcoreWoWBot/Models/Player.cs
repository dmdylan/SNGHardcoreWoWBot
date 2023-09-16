using Postgrest.Attributes;
using Postgrest.Models;

namespace SNGHardcoreWoWBot.Models
{
    [Table("players")]
    internal class Player : BaseModel
    {
        [PrimaryKey("discord_id", true)]
        public ulong DiscordID { get; set; }

        [Column("discord_name")]
        public string DiscordName { get; set; } = "";
    }
}