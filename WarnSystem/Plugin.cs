using Synapse.Api;
using Synapse.Api.Plugin;
using Synapse.Translation;

namespace WarnSystem
{
    [PluginInformation(
        Name = "WarnSystem",
        Author = "Antoniofo",
        Description = "Add a warn system",
        LoadPriority = 0,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = SynapseController.SynapsePatch,
        Version = "v1.0.0"
        )]
    public class Plugin : AbstractPlugin
    {
        public const string WarnsDataKey = "Warn";

        [SynapseTranslation]
        public static new SynapseTranslation<PluginTranslation> Translation { get; set; }
        
        [Config(section = "WarnSystem")]
        public static PluginConfig Config { get; set; }

        public override void Load()
        {
            base.Load();
            Translation.AddTranslation(new PluginTranslation());
            Translation.AddTranslation(new PluginTranslation
            {
                WarnSuccess = "Le joueur %player% a reçu un avertissement pour %reason%",
                Remove = "Avertissement retiré",
                PlayerNotFound = "Joueur non trouvé",
                ArgsError = "Argument insuffusant",
                TypeError = "Paramètre Invalide essayez avec see/add/remove",
                NoWarn = "Le joueur n'a pas d'avertissement",
                WarnNotFound = "Avertissement non trouvé",
                CommandDisable = "Commande désactivé"

            }, "FRENCH");
            Translation.AddTranslation(new PluginTranslation
            {
                WarnSuccess = "Spieler %player% erhielt eine Verwarnung für %reason%",
                Remove = "Werbung entfernt",
                PlayerNotFound = "Spieler nicht gefunden",
                ArgsError = "Nicht genug Argumente",
                TypeError = "Ungültiger Parameter Versuch mit see/add/remove",
                NoWarn = "Der Spieler hat keine Warnung",
                WarnNotFound = "Warnung nicht gefunden",
                CommandDisable = "Befehl deaktiviert"
            }, "GERMAN");
            
        }

        public static int GetNumberOfWarns(Player player)
            => int.Parse(player.GetData(WarnsDataKey) ?? "0");
        

        public static void AddWarn(Player player, string reason)
        {
            var newNumberWarns = (GetNumberOfWarns(player) + 1).ToString();

            player.SetData(WarnsDataKey, newNumberWarns);
            player.SetData(WarnsDataKey + newNumberWarns, reason);

        }

        public static string SeeWarn(Player player, int id)
            => player.GetData(WarnsDataKey + id);
        

        public static string SeeWarns(Player player)
        {
            string output = $"\n{player.NickName} :\n";

            for (int id = 1; id <= GetNumberOfWarns(player); id++)
                output += $"{id} : {player.GetData(WarnsDataKey + id)}\n";

            return output;
        }

        public static bool RemoveWarn(Player player, int id)
        {
            int j = id + 1;
            if (player.GetData(WarnsDataKey + id) == null)
                return false;

            for (int i = id; i <= Plugin.GetNumberOfWarns(player); i++, j++)
            {
                player.SetData(i.ToString(), player.GetData(j.ToString()));
            }
            return true;
        }

    }
}
