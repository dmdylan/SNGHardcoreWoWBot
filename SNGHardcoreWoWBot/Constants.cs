using DSharpPlus.Entities;

namespace SNGHardcoreWoWBot
{
    internal static class Constants
    {
        public static string[] ClassNamesArray { get; private set; } = { "Druid", "Hunter", "Mage", "Paladin", "Priest", "Rogue", "Shaman", "Warlock", "Warrior" };
        public static string[] RaceNamesArray { get; private set; } = { "Human", "Gnome", "Dwarf", "NightElf", "Troll", "Orc", "Tauren", "Undead" };

        #region Class Color Hex Codes

        public static DiscordColor DruidClassColorHexCode { get; private set; } = new DiscordColor("#FF7C0A");
        public static DiscordColor HunterClassColorHexCode { get; private set; } = new DiscordColor("#AAD372");
        public static DiscordColor MageClassColorHexCode { get; private set; } = new DiscordColor("#3FC7EB");
        public static DiscordColor PaladinClassColorHexCode { get; private set; } = new DiscordColor("#F48CBA");
        public static DiscordColor PriestClassColorHexCode { get; private set; } = new DiscordColor("FFFFFF");
        public static DiscordColor RogueClassColorHexCode { get; private set; } = new DiscordColor("#FFF468");
        public static DiscordColor ShamanClassColorHexCode { get; private set; } = new DiscordColor("#0070DD");
        public static DiscordColor WarlockClassColorHexCode { get; private set; } = new DiscordColor("#8788EE");
        public static DiscordColor WarriorClassColorHexCode { get; private set; } = new DiscordColor("#C69B6D");

        #endregion Class Color Hex Codes

        #region Class Names

        public static string DruidClassName { get; private set; } = "Druid";
        public static string HunterClassName { get; private set; } = "Hunter";
        public static string MageClassName { get; private set; } = "Mage";
        public static string PaladinClassName { get; private set; } = "Paladin";
        public static string PriestClassName { get; private set; } = "Priest";
        public static string RogueClassName { get; private set; } = "Rogue";
        public static string ShamanClassName { get; private set; } = "Shaman";
        public static string WarlockClassName { get; private set; } = "Warlock";
        public static string WarriorClassName { get; private set; } = "Warrior";

        #endregion Class Names

        #region Race Names

        public static string HumanRaceName { get; private set; } = "Human";
        public static string GnomeRaceName { get; private set; } = "Gnome";
        public static string DwarfRaceName { get; private set; } = "Dwarf";
        public static string NightElfRaceName { get; private set; } = "NightElf";
        public static string TrollRaceName { get; private set; } = "Troll";
        public static string OrcRaceName { get; private set; } = "Orc";
        public static string TaurenRaceName { get; private set; } = "Tauren";
        public static string UndeadRaceName { get; private set; } = "Undead";

        #endregion Race Names
    }
}