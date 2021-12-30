using Synapse;
using Synapse.Api;
using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Api.Plugin;
using Synapse.Database;
using Synapse.Translation;
using WarnSystem.DataBase;

/*                      /!\ Warning /!\ 
 * The synapse dll is not the synapse from the nugets
 * but the synapse form tempLib because it is not the right vection !
 * [Fr]
 * La dll synapse n'est pas à la bonne verstion donc j'ai pris
 * un dll de tempLib qui est a suprimer lorsque les nugets seront a jours !
 * 
 * @Antoniofo
 */                         

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
        [API]
        public static WarnPlayerRepository WarnRepository { get; private set; }

        [SynapseTranslation]
        public static new SynapseTranslation<PluginTranslate> Translation { get; set;}

        [Config(section = "WarnSystem")]
        public static PluginConfig Config { get; set; }

        public static bool DataBaseEnabled { get; private set; }

        public override void Load()
        {
            DatabaseManager.CheckEnabledOrThrow();
            

            WarnRepository = new WarnPlayerRepository();
            DataBaseEnabled = true;
            Server.Get.Events.Player.PlayerJoinEvent += OnJoin;
            
            InitTranslation();

            base.Load();
        }

        private void InitTranslation()
        {
            Translation.AddTranslation(new PluginTranslate());
            Translation.AddTranslation(new PluginTranslate
            {
                WarningMessage = "<color=red>Attention certaine de vos données seront stocker pour la sécuriter du server.\n Contactez le staff du server pour plus d'informations</color>"
            }, "FRENCH");
            Translation.AddTranslation(new PluginTranslate
            {
                WarningMessage = "<color=red>Bitte beachten Sie, dass einige Ihrer Daten gespeichert werden, um sie vom Server abzusichern.\n Kontaktieren Sie das Serverpersonal für weitere Informationen</color>"
            }, "GERMAN");
        }

        private void OnJoin(PlayerJoinEventArgs ev)
        {
            if (WarnRepository.TryGetByUserId(ev.Player.UserId, out WarnDbo dbo))
            {
                if (dbo.NickName != ev.Player.NickName)
                {
                    dbo.NickName = ev.Player.NickName;
                    WarnRepository.UpdateOrAdd(dbo);
                }
            }
            else
            {
                dbo = new WarnDbo(ev.Player.UserId)
                {
                    NickName = ev.Player.NickName
                };
                WarnRepository.UpdateOrAdd(dbo);

                // do the warning message for eu server
                ev.Player.OpenReportWindow(Translation.ActiveTranslation.WarningMessage);
            }
        }
    }
}
