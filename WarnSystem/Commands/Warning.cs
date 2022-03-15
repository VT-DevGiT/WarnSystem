using Synapse.Api;
using Synapse.Command;
using Unity.Collections.LowLevel.Unsafe;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "warning",
        Aliases = new[] {"wrng"},
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
                if (Warn.GetNumberOfData(player) == 0)
                {
                    result.Message = Plugin.Translation.ActiveTranslation.NoWarn;
                    result.State = CommandResultState.Error;
                }
                else
                {
                    string output = $"\n{player.NickName} :\n";
                            
                    for (int i = 1; i <= Warn.GetNumberOfData(player); i++) 
                    { 
                        output += i+ ": " + player.GetData(i+"")+"\n";
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