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
        Permission = "ws",
        Platforms = new[] {Platform.RemoteAdmin, Platform.ServerConsole},
        Usage = "warn see <PlayerId> | warn add <PlayerId> Warned | warn remove <PlayerId> <WarnID>"
    )]
    public class Warn : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();
            
            //Test if there is enough args
            if (context.Arguments.Count >= 3 || (context.Arguments.Count >= 1 && context.Arguments.Array[1] == "see"))
            {
                //if warn see serverconsole
                if (context.Platform == Platform.ServerConsole && context.Arguments.Array[1] == "see" && context.Arguments.Count == 1)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.PlayerNotFound;
                    result.State = CommandResultState.Error;
                    return result;
                }
                //define parameter of the command
                var cmdType = context.Arguments.Array[1];
                var arguments = "";
                Player player = context.Arguments.Count == 1 ? context.Player : Server.Get.GetPlayer(context.Arguments.Array[2]);

                //gets all the final parameter
                for (int i = 3; i < context.Arguments.Array.Count(); i++)
                    arguments += context.Arguments.Array [i] + " ";
                if (player is null)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.PlayerNotFound;
                    result.State = CommandResultState.Error;
                }
                else
                {
                    int numberOfWarn = Plugin.GetNumberOfWarns(player);
                    switch (cmdType)
                    {
                        case "add":
                            if (context.Player.HasPermission("ws.add"))
                            {
                                var message = Plugin.Translation.ActiveTranslation.WarnSuccess;
                                message = Regex.Replace(message, "%reason%", arguments,       RegexOptions.IgnoreCase);
                                message = Regex.Replace(message, "%player%", player.NickName, RegexOptions.IgnoreCase);
                                string bcMessage = Regex.Replace(Plugin.Translation.ActiveTranslation.PlayerMessage, "%reason%", arguments, RegexOptions.IgnoreCase);
                                player.SendBroadcast(10, bcMessage);
                                Plugin.AddWarn(player, arguments);
                                result.State = CommandResultState.Ok;
                                result.Message = message;    
                            }
                            else
                            {
                                result.State = CommandResultState.Error;
                                result.Message = Plugin.Translation.ActiveTranslation.NoPermission;
                            }
                            break;
                        case "remove" when numberOfWarn == 0:
                        case "see" when numberOfWarn == 0:
                            result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                            result.State = CommandResultState.Error;
                            break;
                        case "see":
                            if (context.Player.HasPermission("ws.see"))
                            {
                                result.Message = Plugin.SeeWarns(player);
                                result.State = CommandResultState.Ok;    
                            }
                            else
                            {
                                result.State = CommandResultState.Error;
                                result.Message = Plugin.Translation.ActiveTranslation.NoPermission;
                            }
                            break;
                        case "remove":
                            if (context.Player.HasPermission("ws.remove"))
                            {
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
                            }
                            else
                            {
                                result.State = CommandResultState.Error;
                                result.Message = Plugin.Translation.ActiveTranslation.NoPermission;
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