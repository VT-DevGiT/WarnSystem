﻿using Synapse.Api.Plugin;
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
    }
}
