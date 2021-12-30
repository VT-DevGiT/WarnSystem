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
        Usage = "Type warn, playerid and the reason",
        Arguments = new[] { "Player" }
        )]
    public class SeeWarn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var Result = new CommandResult();
            var player = Server.Get.GetPlayer(context.Arguments.Array[1]);
            WarnDbo dbo = null;

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

            Result.Message = $"Warns of the player {player} :";
            Result.State = CommandResultState.Ok;

            foreach (var warn in dbo.Warns)
                Result.Message += $"\n{warn}";

            return Result;
        }
    }
}
