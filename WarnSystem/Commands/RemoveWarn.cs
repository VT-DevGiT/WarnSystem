using Synapse.Command;
using Synapse.Api;
using Synapse;
using System.Linq;
using WarnSystem.DataBase;


namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "removewarn",
        Aliases = new string[] { "unwarn"},
        Description = "remove a warn to a player",
        Permission = "ws.warn",
        Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "Type warn, playerid and the reason",
        Arguments = new[] {"Player", "WarnID" }
        )]
    public class RemoveWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();
            var player = Server.Get.GetPlayer(context.Arguments.Array[1]);
            int warnid = 0;
            WarnDbo dbo = null;
            Warn warn = null;

            if (!Plugin.DataBaseEnabled)
            {
                Result.Message = "Database not activated. Contact your host";
                Result.State = CommandResultState.Error;
                return Result;
            }

            if (!int.TryParse(context.Arguments.Array[2], out warnid))
            {
                Result.Message = "Invalide WarnId, it was a number";
                Result.State = CommandResultState.Error;
                return Result;
            }

            if (player != null && Plugin.WarnRepository.TryGetByUserId(player.UserId, out dbo))
            {
                Result.Message = "Invalid player !";
                Result.State = CommandResultState.Error;
                return Result;
            }

            if ((warn = dbo.Warns.FirstOrDefault(w => w.Id == warnid)) != null)
            {
                Result.Message = "No warn with this id !";
                Result.State = CommandResultState.Error;
                return Result;
            }

            dbo.Warns.Remove(warn);

            Result.Message = $"The player {player} no longer this warn";
            Result.State = CommandResultState.Ok;

            return Result;
        }
    }
}
