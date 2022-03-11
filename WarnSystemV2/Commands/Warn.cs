using System.Linq;
using Synapse;
using Synapse.Api;
using Synapse.Command;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "warn",
        Aliases = new[] {"wrn"},
        Arguments = new[] {"see/add/remove", "SteamId", "Additional parameter"},
        Description = "Manage Warn for a player",
        Permission = "ws.warn",
        Platforms = new[] {Platform.RemoteAdmin, Platform.ServerConsole},
        Usage = "warn see Exemple@steam | warn add Exempl@steam Warned | warn remove Exemple@steam 1"
    )]
    public class Warn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();

            //Test if there is enough args
            if (context.Arguments.Count >= 2)
            {
                //define parameter of the command
                var type = context.Arguments.Array[1];
                var playerStr = context.Arguments.Array[2];
                var endArgs = "";
                //gets all the final parameter
                for (int i = 3; i < context.Arguments.Array.Count(); i++)
                {
                    endArgs += context.Arguments.Array[i] + " ";
                }
 
                // temp logger
                Logger.Get.Info($"Type: {type}");
                Logger.Get.Info($"player: {playerStr}");
                Logger.Get.Info($"endArgs: {endArgs}");

                //getting the player
                Player player = GetPlayerBySteamId(playerStr);
                if (player == null)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.PlayerNotFound;
                    result.State = CommandResultState.Error;
                }
                else
                {
                    switch (type)
                    {
                        case "see":
                            var output = "\n";
                            try
                            {
                                for (int i = 1; i < 99; i++)
                                {
                                    output += player.GetData($"{i}");
                                    output+= "\n";
                                }
                            }
                            catch (System.Exception) { }
                            result.Message = output;
                            result.State = CommandResultState.Ok;
                            break;
                        case "add":
                            player.SetData(GetNumberOfData(player) + 1 + "", endArgs);
                            result.State = CommandResultState.Ok;
                            result.Message = Plugin.Translation.ActiveTranslation.Warned.Replace("%reason%", endArgs);
                            player.SendBroadcast(10, Plugin.Translation.ActiveTranslation.WarnSuccess.Replace("%player%", player.NickName).Replace("%reason%", endArgs));
                            break;
                        case "remove":
                            var input = endArgs.Substring(0, 1);
                            int j = int.Parse(input) + 1;
                            for (int i = int.Parse(input); i < GetNumberOfData(player); i++)
                            {
                                if (player.GetData(j + "") == null)
                                {
                                    player.SetData("" + i, "");
                                }
                                else
                                {
                                    player.SetData("" + i, player.GetData("" + j));
                                }
                                j++;
                            }
                            result.State = CommandResultState.Ok;
                            result.Message = Plugin.Translation.ActiveTranslation.Remove;
                            break;
                        default:
                            result.State = CommandResultState.Error;
                            result.Message = $"\n{Plugin.Translation.ActiveTranslation.TypeError}";
                            break;
                    }
                }
            }
            else
            {
                result.Message = Plugin.Translation.ActiveTranslation.ArgsError;
                result.State = CommandResultState.Error;
            }

            return result;
        }

        private int GetNumberOfData(Player ply)
        {
            int count = 0;
            try
            {
                for (int i = 1; i < 99; i++)
                {
                    ply.GetData($"{i}");
                    count++;
                }
            }
            catch (System.Exception)
            {
            }

            return count;
        }

        private Player GetPlayerBySteamId(string steamID)
        {
            try
            {
                Player player = (Player) Server.Get.Players.Where(x => x.UserId == steamID);
                return player;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}