using Synapse.Command;
using Synapse.Api;
using Synapse;
using System.Linq;
using WarnSystem.DataBase;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "addwarn",
        Aliases = new string[] { "warn" },
        Description = "add a warn to a player",
        Permission = "ws.warn",
        Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "add a warn on a player for a specific reason",
        Arguments = new [] { "Player", "Reason" }
        )]
    public class AddWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();
            var player = Server.Get.GetPlayer(context.Arguments.Array[1]);
            string reason = "";
            WarnDbo dbo = null;

            for (int i = 2; i >= context.Arguments.Array.Length; i++)
                reason += $" {context.Arguments.Array[i]}";


            if (!Plugin.DataBaseEnabled)
            {
                Result.Message = "Database not activated. Contact your host";
                Result.State = CommandResultState.Error;
                return Result;
            }

            if (player != null && Plugin.WarnRepository.TryGetByUserId(player.UserId, out dbo))
            {
                Result.Message = "Invalid player !";
                Result.State = CommandResultState.Error;
                return Result;
            }

            dbo.Warns.Add(new Warn()
            {
                ExpiratonDate = System.DateTime.Now,
                Reason = reason,
                WarnDate = System.DateTime.Now,
                Id = dbo.Warns.Count(),
                StaffNickName = context.Player.NickName,
                StaffUserId = context.Player.UserId
            }) ;

            Plugin.WarnRepository.UpdateOrAdd(dbo);

            Result.Message = $"The player {player} get this warn";
            Result.State = CommandResultState.Ok;

            return Result;
        }
    }
}
