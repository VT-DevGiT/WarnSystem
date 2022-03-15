using Synapse.Translation;
using System.ComponentModel;

namespace WarnSystem
{
    public class PluginTranslation : IPluginTranslation
    {
        [Description("Message send to the player when he is warned. %reason% give the reason of its warn")]
        public string Warned { get; set; } = "You have been warned for %reason%";

        [Description("Message send when command completed successfully. %player% is the player name and %reason% is the reason of the warn")]
        public string WarnSuccess { get; set; } = "You have warned %player% for %reason%"; 

        public string Remove { get; set; } = "Warn removed successfully";

        public string PlayerNotFound { get; set; } = "Player not found";

        public string ArgsError { get; set; } = "Not enough arguments";

        public string TypeError { get; set; } = "Invalid Parameter try with see/add/remove";

        public string NoWarn { get; set; } = "The player has no warn";

        public string WarnNotFound { get; set; } = "Warn not found";
    }
}