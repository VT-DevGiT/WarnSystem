using Synapse.Command;
using Synapse.Api;
using Synapse;
using System.Linq;
using WarnSystem.DataBase;


namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "removewarn",
        Aliases = new string[] { },
        Description = "add a warn to a player",
        Permission = "ws.warn",
        Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "Type warn, playerid and the reason"
        )]
    public class RemoveWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();

            var playerid = context.Arguments.Array[1];
            var reason = context.Arguments.Array[2];
            var warnid = context.Arguments.Array[3];


            var P = Server.Get.Players.Where(x => x.PlayerId.Equals(playerid));
            Player player = P.First();
            if (Plugin.DataBaseEnabled)
            {
                var dbo = Plugin.WarnRepo.GetByPlayerId(player.UserId);

                dbo.Data.Remove(int.Parse(warnid));


                WarnPlayerRepo.Update(dbo);

                Result.Message = "Warn removed";
                Result.State = CommandResultState.Ok;
            }
            else
            {
                Result.Message = "Database not activated. Contact your host";
                Result.State = CommandResultState.Error;
            }
            return Result;
        }
    }
}
