using Synapse.Api;
using Synapse.Database;
using System.Collections.Generic;

namespace WarnSystem.DataBase
{
    [API]
    public class WarnDbo : IDatabaseEntity
    {
        public int Id { get; set; }

        public string NickName { get; set; }

        public string GameIdentifier { get; }

        public HashSet<Warn> Warns { get; }

        public int GetId() => Id;

        public WarnDbo(string userId)
        {
            GameIdentifier = userId;
            Warns = new HashSet<Warn>();
        }
    }
}
