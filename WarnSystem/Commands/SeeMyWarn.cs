using Synapse.Api;
using Synapse.Command;

namespace WarnSystem.Commands
{
    [CommandInformation(
        Name = "seemywarn",
        Aliases = new[] {"seewarn", "mywarn"},
        Description = "see your own warn",
        Platforms = new[] {Platform.ClientConsole},
        Usage = "type seemywarn in the console"
    )]
    public class SeeMyWarn : ISynapseCommand
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

                    output += Plugin.SeeWarns(player);

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