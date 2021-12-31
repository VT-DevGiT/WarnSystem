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
        #region Method - Async
        private static readonly object _dbLock = new object();


        public WarnDbo GetByUserId(string id)
        {
            return ExecuteAsync(() => Get(LiteDB.Query.EQ(nameof(WarnDbo.GameIdentifier), id)));
        }
        public WarnDbo GetByNickName(string name)
        {
            return ExecuteAsync(() => Get(LiteDB.Query.EQ(nameof(WarnDbo.NickName), name)));
        }


        public bool ExisteByUserId(string id)
        {
            return ExecuteAsync(() => Exists(LiteDB.Query.EQ(nameof(WarnDbo.GameIdentifier), id)));
        }

        public bool ExisteByNickName(string name)
        {
            return Exists(LiteDB.Query.EQ(nameof(WarnDbo.NickName), name));
        }


        private static T ExecuteAsync<T>(Func<T> funcFill)
        {
            Task<T> task = Task.Run(() =>
            {
                lock (_dbLock)
                {
                    T result = funcFill.Invoke();
                    return result;
                }
            });
            return task.Result;
        }
        #endregion

        #region Methods
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
            if (ExisteByUserId(item.GameIdentifier))
                Save(item);
            else
                Insert(item);
        }
        #endregion
    }
}
