using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;

namespace WarnSystem
{
    [Automatic]
    public class PluginTranslation : Translations<PluginTranslation>
    {
        public string WarnSuccess { get; set; } = "You have warned {0} for {1}"; 

        public string Remove { get; set; } = "Warn removed successfully";

        public string PlayerNotFound { get; set; } = "Player not found";

        public string ArgsError { get; set; } = "Not enough arguments";

        public string TypeError { get; set; } = "Invalid Parameter try with see/add/remove";

        public string NoWarn { get; set; } = "The player has no warn";

        public string WarnNotFound { get; set; } = "Warn not found";

        public string CommandDisable { get; set; } = "Command disabled";

        public string WarnDnt { get; set; } = "The server need to store data of you for security reason";

        public string PlayerMessage { get; set; } = "You have been warned for : \"<color=red>{0}</color>\"";
        
        public string NoPermission { get; set; } = "You don't have permission to use this command";
        
    }
}