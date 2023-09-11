using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNGHardcoreWoWBot.Commands
{
    public class BaseCommands : BaseCommandModule
    {
        [Command("register")]
        public async Task RegisterPlayerCommand(CommandContext commandContext)
        {
            await commandContext.RespondAsync($"Registered {commandContext.User.Username}");
        }

        [Command("addcharacter")]
        public async Task RegisterPlayerCharacterCommand(CommandContext commandContext, string characterName)
        {
            await commandContext.RespondAsync($"{characterName} added for {commandContext.User.Username}");
        }

        [Command("characterstatus")]
        public async Task ChangeCharacterStatus(CommandContext commandContext, string characterName, string status)
        {
            await commandContext.RespondAsync($"{characterName} {status}");
        }
    }
}
