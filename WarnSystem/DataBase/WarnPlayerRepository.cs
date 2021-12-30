using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synapse.Api;
using Synapse.Database;

namespace WarnSystem.DataBase
{
    [API]
    public class WarnPlayerRepository : Repository<WarnDbo>
    {
        private static readonly object _dbLock = new object();

        public WarnDbo GetByUserId(string id) => Get(LiteDB.Query.EQ(nameof(WarnDbo.GameIdentifier), id));
        
        public WarnDbo GetByNickName(string name) => Get(LiteDB.Query.EQ(nameof(WarnDbo.NickName), name));


        public bool ExisteByUserId(string id) => Exists(LiteDB.Query.EQ(nameof(WarnDbo.GameIdentifier), id));

        public bool ExisteByNickName(string name) => Exists(LiteDB.Query.EQ(nameof(WarnDbo.NickName), name));


        public bool TryGetByUserId(string id, out WarnDbo warnDbo)
        {
            if (ExisteByUserId(id))
            {
                warnDbo = GetByUserId(id);
                return true;
            }
            else
            {
                warnDbo = null;
                return false;
            }
        }

        public bool TryGetByNickName(string name, out WarnDbo warnDbo)
        {
            if (ExisteByNickName(name))
            {
                warnDbo = GetByNickName(name);
                return true;
            }
            else
            {
                warnDbo = null;
                return false;
            }
        }

        public void UpdateOrAdd(WarnDbo item)
        {
            new Task(() =>
            {
                lock (_dbLock)
                {
                    if (ExisteByUserId(item.GameIdentifier))
                        Save(item);
                    else
                        Insert(item);
                }
            }
            ).Start();
        }
    }
}
