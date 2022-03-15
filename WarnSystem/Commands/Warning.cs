using Synapse.Api;
using Synapse.Command;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "warning",
        Aliases = new[] {"wrng", "mywarn"},
        Description = "see your own warn",
        Platforms = new[] {Platform.ClientConsole},
        Usage = "warning"
    )]
    public class Warning : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();

            if (Plugin.Config.PlayerCommand)
            {
                Player player = context.Player;
                if (Plugin.GetNumberOfWarns(player) == 0)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                    result.State = CommandResultState.Error;
                }
                else
                {
                    string output = $"\n{player.NickName} :\n";
                            
                    for (int i = 1; i <= Plugin.GetNumberOfWarns(player); i++) 
                    { 
                        output += $"{i} : {player.GetData(i.ToString())} \n";
                    }
                    result.Message = output;
                    result.State = CommandResultState.Ok;    
                }
            }
            else
            {
                result.Message = Plugin.Translation.ActiveTranslation.CommandDisable;
                result.State = CommandResultState.Error;
            }
            
            return result;
        }
    }
}