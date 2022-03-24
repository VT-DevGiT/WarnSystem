using Synapse.Translation;

namespace WarnSystem
{
    public class PluginTranslation : IPluginTranslation
    {
        public string WarnSuccess { get; set; } = "You have warned %player% for %reason%"; 

        public string Remove { get; set; } = "Warn removed successfully";

        public string PlayerNotFound { get; set; } = "Player not found";

        public string ArgsError { get; set; } = "Not enough arguments";

        public string TypeError { get; set; } = "Invalid Parameter try with see/add/remove";

        public string NoWarn { get; set; } = "The player has no warn";

        public string WarnNotFound { get; set; } = "Warn not found";

        public string CommandDisable { get; set; } = "Command disabled";

        public string WarnDnt { get; set; } = "WarnSystem is gonna store data with you (its the warns)";
    }
}