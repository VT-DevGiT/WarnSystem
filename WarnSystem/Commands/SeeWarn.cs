using Synapse.Command;
using Synapse.Api;
using Synapse;
using System.Linq;
using WarnSystem.DataBase;


namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "seewarn",
        Aliases = new string[] { },
        Description = "see warn of a player (or yours)",
        Permission = "",
        Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole, Platform.ClientConsole },
        Usage = "Type warn, playerid and the reason"
        )]
    public class SeeWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();

            var id = context.Arguments.Array[1];
            var reason = context.Arguments.Array[2];
            var warnid = context.Arguments.Array[3];

            Logger.Get.Info("1");

            string output = $"Warn of {Server.Get.Players.Where(x=> x.PlayerId == int.Parse(id))} \n";
            Logger.Get.Info("2");
            var P = Server.Get.Players.Where(x => x.PlayerId.Equals(id));
            Player player = P.First();
            if (Plugin.DataBaseEnabled)
            {
                Logger.Get.Info("db in");
                var dbo = Plugin.WarnRepo.GetByPlayerId(player.UserId);

                if(dbo.Data.TryGetValue(int.Parse(warnid), out Warn warn) && context.Player.HasPermission("ws.warn"))
                {

                }
                else
                {
                    foreach (var item in dbo.Data.Values)
                    {
                        output += $"ID: {dbo.Data.FirstOrDefault(x=> x.Value == item).Key}, {item}\n";
                    }
                        

                }


                Result.Message = output;
                Result.State = CommandResultState.Ok;
            }
            else
            {
                Logger.Get.Info("db out");
                Result.Message = "Database not activated. Contact your host";
                Result.State = CommandResultState.Error;
            }
            return Result;
        }
    }
}
