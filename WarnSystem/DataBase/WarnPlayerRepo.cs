using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synapse.Database;

namespace WarnSystem.DataBase
{
    public class WarnPlayerRepo : Repository<Warndbo>
    {
        public Warndbo GetByName(string name)
        {
            return Get(LiteDB.Query.EQ("Name", name));
        }

        public Warndbo GetByPlayerId(string id)
        {
            return Get(LiteDB.Query.EQ("UserId", id));
        }

        public bool ExistPlayerID(string id)
        {
            return Exists(LiteDB.Query.EQ("UserId", id));
        }

        public static void AddDbo(Warndbo item)
        {
            Plugin.WarnRepo.Insert(item);
        }

        public static void Update(Warndbo item)
        {
            Plugin.WarnRepo.Save(item);
        }
    }
}
