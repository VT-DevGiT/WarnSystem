using Synapse.Config;
using Synapse.Translation;
using System.ComponentModel;

namespace WarnSystem
{
    public class PluginConfig : AbstractConfigSection
    {
        //mettre date expiration configurable

    }

    public class PluginTranslate : IPluginTranslation
    {
        [Description("This is a waring message for every new player (Don't change this for your own security Its EULA of SCP:SL Stuff) EUROPEAN ONLY")]
        public string WarningMessage { get; set; } = "<color=red>Warning some of your information we be saved by the server. \n For more info contact your server owner</color>";
    }
}
