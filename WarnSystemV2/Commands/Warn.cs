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
            if (context.Arguments.Count >= 3 || context.Arguments.Array[1] == "see")
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
                            if (GetNumberOfData(player) == 0)
                            {
                                result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                                result.State = CommandResultState.Error;
                            }
                            else
                            {
                                string output = $"\n{player.NickName} :\n";
                            
                                for (int i = 1; i <= GetNumberOfData(player); i++) 
                                { 
                                    output += i+ ": " + player.GetData(i+"")+"\n";
                                }
                                result.Message = output;
                                result.State = CommandResultState.Ok;    
                            }
                            break;
                        case "add":
                            player.SetData(GetNumberOfData(player) + 1+"", endArgs);
                            result.State = CommandResultState.Ok;
                            result.Message = Plugin.Translation.ActiveTranslation.WarnSuccess.Replace("%reason%", endArgs).Replace("%player%", player.NickName);
                            player.SendBroadcast(10, Plugin.Translation.ActiveTranslation.Warned.Replace("%reason%", endArgs));
                            break;
                        case "remove":
                            if (GetNumberOfData(player) == 0)
                            {
                                result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                                result.State = CommandResultState.Error;
                            }
                            else
                            {
                                int input = int.Parse(endArgs);
                                int j = input + 1;
                                if (player.GetData(input+"") == null)
                                {
                                    result.Message = Plugin.Translation.ActiveTranslation.WarnNotFound;
                                    result.State = CommandResultState.Error;
                                    break;
                                }
                                for (int i = int.Parse(input+""); i <= GetNumberOfData(player); i++)
                                {
                                    if (player.GetData(j + "") == null)
                                    {
                                        player.SetData("" + i, null);
                                    }
                                    else
                                    {
                                        player.SetData("" + i, player.GetData("" + j));
                                    }
                                    j++;
                                }
                                result.State = CommandResultState.Ok;
                                result.Message = Plugin.Translation.ActiveTranslation.Remove;    
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

        public static int GetNumberOfData(Player ply)
        {
            int count = 0;
            for (int i = 1; i > -1; i++)
            {
                if (ply.GetData(i + "") == null)
                {
                    break;
                }
                count++;
            }
            return count;
        }

        private Player GetPlayerBySteamId(string steamID)
        {
            try
            {
                Player player = Server.Get.GetPlayerByUID(steamID);
                return player;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}