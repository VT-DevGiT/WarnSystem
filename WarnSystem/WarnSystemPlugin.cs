using Neuron.Core.Events;
using Neuron.Core.Meta;
using Neuron.Core.Plugins;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Events;
using Synapse3.SynapseModule.Player;
using WarnSystemModule;

namespace WarnSystem
{
    [Plugin(
    Name = "WarnSystem",
    Description = "Add a warn system",
    Version = "2.0.0",
    Author = "Antoniofo"
    )]
    public class WarnSystemPlugin : ReloadablePlugin<PluginConfig, PluginTranslation>
    {

    }

    [Automatic]
    public class EventsHandler : Listener
    {
        private readonly WarnSystemPlugin _plugin;
        private readonly WarnService _warn;

        public EventsHandler(WarnSystemPlugin plugin, WarnService warn)
        {
            _plugin = plugin;
            _warn = warn;
        }

        private void OnJoin(JoinEvent ev)
        {
            MEC.Timing.CallDelayed(0.1f, () => OnJoinDelayed(ev.Player));
        }

        private void OnJoinDelayed(SynapsePlayer player)
        {
            if (!_plugin.Config.DisclamerAtFirstConnection)
                return;

            if (!_warn.WarnIsSet(player))
            {
                _warn.SetNumberOfWarns(player, 0);

                player.SendBroadcast(player.GetTranslation(_plugin.Translation).WarnDnt, 5);
            }
        }
    }
}
