using System.Linq;
using Synapse;
using Synapse.Api;
using Synapse.Command;
using Synapse.Command.Commands;

namespace WarnSystem.Warn
{
  [CommandInformation(
    Name = "warn",
    Aliases = new string[] { "wrn" },
    Arguments = new string[] { "see/add/remove", "player", "aditional parameter" },
    Description = "Manage Warn for a player",
    Permission = "ws.warn",
    Platforms = new[] { Platform.RemoteAdmin, Platform.ServerConsole },
    Usage = "warn see Exemple@steam \nwarn add Exempl@steam Warned \nwarn remove Exemple@steam 1"
  )]
  public class Warn : SynapsePluginCommand
  {
    public new CommandResult Execute(CommandContext context)
    {
      var result = new CommandResult();

      //define parameter of the command
      var type = context.Arguments.Array[1];
      var playerStr = context.Arguments.Array[2];
      var endArgs = "";
      //gets all the final parameter
      for (int i = 3; i < context.Arguments.Array.Count(); i++)
      {
        endArgs.Concat(context.Arguments.Array[i] + " ");
      }
      // temp logger
      Logger.Get.Info($"Type: {type}");
      Logger.Get.Info($"player: {playerStr}");
      Logger.Get.Info($"endArgs: {endArgs}");
      //
      Player player = Server.Get.GetPlayer(playerStr);
      switch (type)
      {
        case "see":
          var output = "";
          try
          {
            for (int i = 1; i < 99; i++)
            {
              output.Concat(player.GetData($"{i}"));
              output.Concat("\n");
            }
          }
          catch (System.Exception) { }
          result.Message = output;
          result.State = CommandResultState.Ok;
          break;
        case "add":
          player.SetData(getNumberOfData(player) + 1 + "", endArgs);
          result.State = CommandResultState.Ok;
          result.Message = $"\nPlayer {player.DisplayName} has been warned for {endArgs}";
          player.SendBroadcast(10, $"You have been wared for {endArgs}");
          break;
        case "remove":
          var input = endArgs.Substring(0, 1);
          int j = int.Parse(input) + 1;
          for (int i = int.Parse(input); i < getNumberOfData(player); i++)
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
          result.Message = "Warn removed successfully";
          break;
        default:
          result.State = CommandResultState.Error;
          result.Message = "\nInvalid Prameter try with see/add/remove";
          break;
      }
      return result;
    }
    public int getNumberOfData(Player ply)
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
      catch (System.Exception) { }

      return count;
    }
  }
}