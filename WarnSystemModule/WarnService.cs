using Neuron.Core.Meta;
using PluginAPI.Core;
using Synapse3.SynapseModule;
using Synapse3.SynapseModule.Database;
using Synapse3.SynapseModule.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarnSystemModule
{
    public class WarnService : Service
    {
        public const string WarnsDataKey = "Warn";


        private readonly DatabaseService _database;
        public WarnService(DatabaseService database)
        {
            _database = database;
        }

        #region Base of Warn Method
        public bool WarnIsSet(SynapsePlayer player)
            => player.GetData(WarnsDataKey) != null;

        public int GetNumberOfWarns(SynapsePlayer player)
        {
            var value = player.GetData(WarnsDataKey);
            if (string.IsNullOrEmpty(value))
                return 0;
            return int.Parse(value);
        }

        public void SetNumberOfWarns(SynapsePlayer player, int value)
            => player.SetData(WarnsDataKey, value.ToString());

        public string SeeWarn(SynapsePlayer player, int id)
            => player.GetData(WarnsDataKey + id);

        public void SetWarn(SynapsePlayer player, int id, string value)
            => player.SetData(WarnsDataKey + id, value);

        #endregion

        #region Warn Method
        public void AddWarn(SynapsePlayer player, string reason)
        {
            var newNumberWarns = GetNumberOfWarns(player) + 1;

            SetNumberOfWarns(player, newNumberWarns);
            SetWarn(player, newNumberWarns, reason);
        }

        public string SeeWarns(SynapsePlayer player)
        {
            int warnCount = GetNumberOfWarns(player);
            string output = $"\n{player.NickName} :\n";

            for (int id = 1; id <= warnCount; id++)
                output += $"{id} : {SeeWarn(player, id)}\n";

            return output;
        }

        public bool RemoveWarn(SynapsePlayer player, int id)
        {
            int warnCount = GetNumberOfWarns(player);

            if (warnCount < id)
                return false;

            SetNumberOfWarns(player, warnCount - 1);

            for (int i = id, j = i + 1; i <= warnCount - 1; i++, j++)
                SetWarn(player, i, i == warnCount ? null : SeeWarn(player, j));

            return true;
        }
        #endregion
    }
}
