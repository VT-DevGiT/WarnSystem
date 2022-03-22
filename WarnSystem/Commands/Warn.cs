using Synapse;
using Synapse.Api;
using Synapse.Command;
using System.Linq;
using System.Text.RegularExpressions;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "warn",
        Aliases = new[] {"wrn"},
        Arguments = new[] {"see/add/remove", "Player", "(reason or warn id)"},
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
            if (context.Arguments.Count >= 3 || context.Arguments.Array[1] == "see")
            {
                //define parameter of the command
                var cmdType = context.Arguments.Array[1];
                var playerStr = context.Arguments.Array[2];
                var arguments = "";
                //gets all the final parameter
                for (int i = 3; i < context.Arguments.Array.Count(); i++)
                {
                    arguments += context.Arguments.Array[i] + " ";
                }

                //getting the player
                Player player = Server.Get.GetPlayer(playerStr);
                if (player == null)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.PlayerNotFound;
                    result.State = CommandResultState.Error;
                }
                else
                {
                    switch (cmdType)
                    {
                        case "remove" when Plugin.GetNumberOfWarns(player) == 0:
                        case "see" when Plugin.GetNumberOfWarns(player) == 0:
                            result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                            result.State = CommandResultState.Error;
                            break;
                        case "see":
                            result.Message = Plugin.SeeWarns(player);
                            result.State = CommandResultState.Ok;
                            break;
                        case "add":
                            var message = Plugin.Translation.ActiveTranslation.WarnSuccess;
                            message = Regex.Replace(message, "%reason%", arguments, RegexOptions.IgnoreCase);
                            message = Regex.Replace(message, "%player%", player.NickName, RegexOptions.IgnoreCase);
                            player.SendBroadcast(10, Plugin.Config.PlayerMessage.Replace("%reason%", arguments));
                            Plugin.AddWarn(player, arguments);
                            result.State = CommandResultState.Ok;
                            result.Message = message;
                            break;
                        case "remove":
                            if (!int.TryParse(arguments, out int id))
                            {
                                result.Message = Plugin.Translation.ActiveTranslation.WarnNotFound;
                                result.State = CommandResultState.Error;
                            }
                            else if (Plugin.RemoveWarn(player, id))
                            {
                                result.State = CommandResultState.Ok;
                                result.Message = Plugin.Translation.ActiveTranslation.Remove;
                            }
                            else
                            {
                                result.Message = Plugin.Translation.ActiveTranslation.WarnNotFound;
                                result.State = CommandResultState.Error;
                            }
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

    }
}