using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNGHardcoreWoWBot
{
    internal static class Constants
    {
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
    }
}