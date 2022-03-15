using System.ComponentModel;
using Synapse.Config;
namespace WarnSystem
{
    public class PluginConfig : IConfigSection
    {
        [Description("Message that the player recieve when he get warned")]
        public string PlayerMessage { get; set; } = "You have been Warned for <color=red>%reason%</color>";

        [Description("Allow the player to use the client console command to his own warn")]
        public bool PlayerCommand { get; set; } = true;
    }
}