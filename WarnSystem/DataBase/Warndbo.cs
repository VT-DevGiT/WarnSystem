using Synapse.Api;
using Synapse.Database;
using System;
using System.Collections.Generic;

namespace WarnSystem.DataBase
{
    [API]
    public class WarnDbo : IDatabaseEntity
    {
        #region Properties & Variable
        public int Id { get; set; }

        public string NickName { get; set; }

        public string GameIdentifier { get; }

        public HashSet<Warn> Warns { get; }
        #endregion

        #region Constructor & Destructor
        public WarnDbo(string userId)
        {
            GameIdentifier = userId;
            Warns = new HashSet<Warn>();
        }
        #endregion

        #region Methods
        public int GetId() => Id;

        public override int GetHashCode()
        {
            return GetType().GetHashCode() ^ ((!String.IsNullOrWhiteSpace(GameIdentifier)) ? Id.GetHashCode() : String.Empty.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as WarnDbo);
        }

        public bool Equals(WarnDbo other)
        {
            return other != null && (this.Id == other.Id);
        }
        #endregion

        #region Operators
        public static bool operator ==(WarnDbo left, WarnDbo right)
        {
            return (left is null) ? (right is null) : left.Equals(right);
        }

        public static bool operator !=(WarnDbo left, WarnDbo right)
        {
            return !(left == right);
        }
        #endregion
    }
}
