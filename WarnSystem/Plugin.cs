using Synapse;
using Synapse.Api.Plugin;
using Synapse.Api;
using Synapse.Database;
using Synapse.Api.Events.SynapseEventArguments;
using System;
using WarnSystem.DataBase;
using Synapse.Translation;

namespace WarnSystem
{
    [PluginInformation(
        Name = "WarnSystem",
        Author = "Antoniofo",
        Description = "Add a warn system",
        LoadPriority = 0,
        SynapseMajor = 2,
        SynapseMinor = 8,
        SynapsePatch = 0,
        Version = "v1.0.0"
        )]
    public class Plugin : AbstractPlugin
    {
        public static WarnPlayerRepo WarnRepo;

        [SynapseTranslation]
        public static new SynapseTranslation<Translate> Translation {get;set;}

        public static bool DataBaseEnabled { get; private set; }

        public override void Load()
        {
            DatabaseManager.CheckEnabledOrThrow();
            base.Load();
            WarnRepo = new WarnPlayerRepo();
            Server.Get.Events.Player.PlayerJoinEvent += OnJoin;

            Translation.AddTranslation(new Translate());
            Translation.AddTranslation(new Translate
            {
                WarningMessage = "<color=red>Attention certaine de vos données seront stocker pour la sécuriter du server.\n Contactez le staff du server pour plus d'informations</color>"
            }, "FRENCH");
            Translation.AddTranslation(new Translate
            {
                WarningMessage = "<color=red>Bitte beachten Sie, dass einige Ihrer Daten gespeichert werden, um sie vom Server abzusichern.\n Kontaktieren Sie das Serverpersonal für weitere Informationen</color>"
            }, "GERMAN");

            DataBaseEnabled = true;

        }

        private void OnJoin(PlayerJoinEventArgs ev)
        {
            if (WarnRepo.ExistPlayerID(ev.Player.UserId))
            {
                Logger.Get.Info("not Already connected once");
                ev.Player.OpenReportWindow(Translation.ActiveTranslation.WarningMessage);
                var dbo = new Warndbo()
                {
                    UserID = ev.Player.UserId,
                    Data = new System.Collections.Generic.Dictionary<int, Warn>(),
                    Name = ev.Player.NickName
                };
                WarnPlayerRepo.AddDbo(dbo);

            }
            else
            {
                Logger.Get.Info("Already connected once");
            }
            
        }

        public override void ReloadConfigs()
        {
            base.ReloadConfigs();
        }

    }
}
