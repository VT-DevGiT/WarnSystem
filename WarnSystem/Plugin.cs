using Synapse.Api;
using Synapse.Api.Plugin;
using Synapse.Database;
using Synapse.Translation;
using System;

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

        #region Plugin Stuff

        [SynapseTranslation]
        public static new SynapseTranslation<PluginTranslation> Translation { get; set; }
        
        [Config(section = "WarnSystem")]
        public static PluginConfig Config { get; set; }

        public override void Load()
        {
            base.Load();
            DatabaseManager.CheckEnabledOrThrow();
            new EventsHandler();

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
                CommandDisable = "Commande désactivé",
                WarnDnt = "Le serveur a besoin de stocker vos données pour des raisons de sécurité"

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
                CommandDisable = "Befehl deaktiviert",
                WarnDnt = "Der Server muss aus Sicherheitsgründen Daten von Ihnen speichern"
            }, "GERMAN");
        }
        #endregion

        #region Base of Warn Method
        public static bool WarnIsSet(Player player)
            => player.GetData(WarnsDataKey) != null;

        public static bool WarnIsSet(PlayerDbo dbo)
            => dbo.Data.ContainsKey(WarnsDataKey);

        public static int GetNumberOfWarns(Player player)
            => int.Parse(player.GetData(WarnsDataKey) ?? "0");

        public static int GetNumberOfWarns(PlayerDbo dbo)
            => int.Parse(dbo.Data.ContainsKey(WarnsDataKey) ? dbo.Data[WarnsDataKey] : "0");

        public static void SetNumberOfWarns(Player player, int value)
            => player.SetData(WarnsDataKey, value.ToString());

        public static void SetNumberOfWarns(PlayerDbo dbo, int value)
            => dbo.Data[WarnsDataKey] = value.ToString();

        public static string SeeWarn(Player player, int id)
            => player.GetData(WarnsDataKey + id);

        public static string SeeWarn(PlayerDbo dbo, int id)
            => dbo.Data.ContainsKey(WarnsDataKey) ? dbo.Data[WarnsDataKey] : "";

        public static void SetWarn(Player player, int id, string value)
            => player.SetData(WarnsDataKey + id, value);

        public static void SetWarn(PlayerDbo dbo, int id, string value)
            => dbo.Data[WarnsDataKey + id] = value;
        #endregion

        #region Warn Method
        public static void AddWarn(Player player, string reason)
        {
            var dbo = DatabaseManager.PlayerRepository.FindByGameId(player.UserId);
            var newNumberWarns = GetNumberOfWarns(dbo) + 1;

            SetNumberOfWarns(dbo, newNumberWarns);
            SetWarn(dbo, newNumberWarns, reason);

            DatabaseManager.PlayerRepository.Save(dbo);
        }
        
        public static string SeeWarns(Player player)
        {
            var dbo = DatabaseManager.PlayerRepository.FindByGameId(player.UserId);
            int warnCount = GetNumberOfWarns(dbo);
            string output = $"\n{player.NickName} :\n";

            for (int id = 1; id <= warnCount; id++)
                output += $"{id} : {SeeWarn(dbo, id)}\n";

            return output;
        }

        public static bool RemoveWarn(Player player, int id)
        {
            var dbo = DatabaseManager.PlayerRepository.FindByGameId(player.UserId);
            int warnCount = GetNumberOfWarns(dbo);

            if (warnCount < id)
                return false;

            SetNumberOfWarns(dbo, warnCount - 1);

            for (int i = id, j = i + 1; i <= warnCount - 1; i++, j++)
                SetWarn(dbo, i, i == warnCount ? null : SeeWarn(dbo, j));

            DatabaseManager.PlayerRepository.Save(dbo);
            return true;
        }
        #endregion
    }
}
