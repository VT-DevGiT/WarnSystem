using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Database;

namespace WarnSystem
{
    public class EventsHandler
    {
        public EventsHandler()
        {
            SynapseController.Server.Events.Player.PlayerJoinEvent += OnJoin;
        }

        private void OnJoin(PlayerJoinEventArgs ev)
        {
            if (!Plugin.Config.DisclamerAtFirstConnection)
                return;

            PlayerDbo dbo = DatabaseManager.PlayerRepository.FindByGameId(ev.Player.UserId);
            if (!Plugin.WarnIsSet(dbo))
            {
                Plugin.SetNumberOfWarns(dbo, 0);
                DatabaseManager.PlayerRepository.Save(dbo);

                ev.Player.SendBroadcast(5, Plugin.Translation.ActiveTranslation.WarnDnt);
            }
        }
    }
}
