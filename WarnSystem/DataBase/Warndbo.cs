using Synapse.Database;
using System.Collections.Generic;

namespace WarnSystem.DataBase
{
    public class Warndbo : IDatabaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string UserID { get; set; }

        public Dictionary<int, Warn> Data { get; set; }

        public int GetId() => Id;
    }
}
