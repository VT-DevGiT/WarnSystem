using Synapse.Command;
using Synapse.Api;
using Synapse;
using System.Linq;
using WarnSystem.DataBase;


namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "addwarn",
        Aliases = new string[] { },
        Description = "add a warn to a player",
        Permission = "ws.warn",
        Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "Type warn, playerid and the reason"
        )]
    public class AddWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();
         
            var id = context.Arguments.Array[1];
            var reason = context.Arguments.Array[2];

            Logger.Get.Info(id);


            var P = Server.Get.Players.Where(x => x.PlayerId.Equals(id));
            var player = P.First();
            if (Plugin.DataBaseEnabled)
            {
                var dbo = Plugin.WarnRepo.GetByPlayerId(player.UserId);

                dbo.Data.Add(dbo.Data.Count()+1,new Warn()
                {
                    ExpiratonDate = System.DateTime.Now,
                    Reason = reason,
                    WarnDate = System.DateTime.Now,
                    Id = dbo.Data.Count()

                }) ;

                WarnPlayerRepo.Update(dbo);

                Result.Message = "Warn Added";
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
