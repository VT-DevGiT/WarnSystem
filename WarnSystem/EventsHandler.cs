using Synapse.Api;
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
            MEC.Timing.CallDelayed(0.1f, () => OnJoinDelayed(ev.Player));
        }

        private void OnJoinDelayed(Player ply)
        {
            if (!Plugin.Config.DisclamerAtFirstConnection)
                return;

            PlayerDbo dbo = DatabaseManager.PlayerRepository.FindByGameId(ply.UserId);
            if (!Plugin.WarnIsSet(dbo))
            {
                Plugin.SetNumberOfWarns(dbo, 0);
                DatabaseManager.PlayerRepository.Save(dbo);

                ply.SendBroadcast(5, Plugin.Translation.ActiveTranslation.WarnDnt);
            }
        }
    }
}
