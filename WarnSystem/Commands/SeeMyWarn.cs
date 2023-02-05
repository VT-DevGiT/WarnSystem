using Neuron.Core.Meta;
using Neuron.Modules.Commands;
using Neuron.Modules.Commands.Command;
using Synapse3.SynapseModule.Command;
using WarnSystemModule;

namespace WarnSystem.Commands
{
    [Automatic]
    [SynapseCommand(
        CommandName = "seemywarn",
        Aliases = new[] {"seewarn", "mywarn"},
        Description = "see your own warn",
        Platforms = new[] { CommandPlatform.PlayerConsole }
    )]
    public class SeeMyWarn : SynapseCommand
    {
        private readonly WarnSystemPlugin _plugin;
        private readonly WarnService _warn;

        public SeeMyWarn(WarnSystemPlugin plugin, WarnService warn)
        {
            _plugin = plugin;
            _warn = warn;
        }

        public override void Execute(SynapseContext context, ref CommandResult result)
        {
            if (_plugin.Config.PlayerCommand)
            {
                var player = context.Player;
                if (_warn.GetNumberOfWarns(player) == 0)
                {
                    result.Response = context.Player.GetTranslation(_plugin.Translation).NoWarn;
                    result.StatusCode = CommandStatusCode.Error;
                }
                else
                {
                    string output = _warn.SeeWarns(player);
                    result.Response = output;
                    result.StatusCode = CommandStatusCode.Ok;
                }
            }
            else
            {
                result.Response = context.Player.GetTranslation(_plugin.Translation).CommandDisable;
                result.StatusCode = CommandStatusCode.Error;
            }
        }
    }
}